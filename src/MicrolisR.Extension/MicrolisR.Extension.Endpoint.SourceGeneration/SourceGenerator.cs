using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MicrolisRExtension.Endpoint.SourceGeneration;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TypeDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (syntaxNode, _) =>
                    IsSyntaxTargetFromGeneration(syntaxNode),
                transform: static (ctx, _) =>
                    GetSemanticTargetForGeneration(ctx))
            .Where(static classDeclaration => classDeclaration is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<TypeDeclarationSyntax>)> compilationAndClasses =
            context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetFromGeneration(SyntaxNode syntaxNode)
        => syntaxNode is ClassDeclarationSyntax;

    private static TypeDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (TypeDeclarationSyntax) context.Node;

        var isType = classDeclarationSyntax
            .BaseList?.Types
            .Select(x => x.Type)
            .OfType<GenericNameSyntax>()
            .Any(x => x.Identifier.Text == "IRequestHandler");

        return isType is false or null ? null : classDeclarationSyntax;
    }

    private static void Execute(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
            return;

        var distinctClasses = classes.Distinct();

        var targetClasses = GetClasses(compilation, context, distinctClasses);

        if (targetClasses.Count < 1)
            return;

        var resourceName = GetAssemblyName(compilation, classes.FirstOrDefault());
        
        var result = Emitter.Emit(targetClasses, context.CancellationToken);
        
        context.AddSource($"{resourceName}HttpContextRequestResolvers.g.cs", SourceText.From(result, Encoding.UTF8));
    }

    private static IReadOnlyList<EndpointClass> GetClasses(Compilation compilation,
        SourceProductionContext context, IEnumerable<TypeDeclarationSyntax> classes)
    {
        var targetClasses = new List<EndpointClass>();

        foreach (var handlerDeclarationSyntax in classes)
        {
            if (context.CancellationToken.IsCancellationRequested)
                return Array.Empty<EndpointClass>();

            var handlerClasses = GetEndpointClasses(compilation, handlerDeclarationSyntax);
            targetClasses.AddRange(handlerClasses);
        }
        
        return targetClasses;
    }

    private static IEnumerable<EndpointClass> GetEndpointClasses(Compilation compilation,
        BaseTypeDeclarationSyntax handlerDeclarationSyntax)
    {
        var requestDeclarationSyntaxList = handlerDeclarationSyntax
            .BaseList?.Types
            .Select(x => x.Type)
            .OfType<GenericNameSyntax>()
            .Where(x => x.Identifier.Text == "IRequestHandler");

        if (requestDeclarationSyntaxList is null)
            return Enumerable.Empty<EndpointClass>();

        var classModel = compilation.GetSemanticModel(handlerDeclarationSyntax.SyntaxTree);

        var endpointClasses = new List<EndpointClass>();

        foreach (var syntax in requestDeclarationSyntaxList)
        {
            var requestFullName = GetClassFullName(classModel, syntax.TypeArgumentList.Arguments.FirstOrDefault());

            if (requestFullName is null)
                continue;

            var responseFullName = GetClassFullName(classModel, syntax.TypeArgumentList.Arguments[1]);

            if (responseFullName is null)
                continue;

            var requestProperties = GetRequestProperties(classModel, syntax.TypeArgumentList.Arguments.FirstOrDefault());

            endpointClasses.Add(new EndpointClass(requestFullName, responseFullName, requestProperties));
        }
        
        return endpointClasses;
    }

    private static string? GetClassFullName(SemanticModel classModel, TypeSyntax? typeSyntax)
    {
        if (typeSyntax is not IdentifierNameSyntax nameSyntax)
            return null;

        var typeSymbol = classModel.GetTypeInfo(nameSyntax).Type;

        return typeSymbol?.ToDisplayString();
    }

    private static IEnumerable<Property> GetRequestProperties(SemanticModel classModel, TypeSyntax? typeSyntax)
    {
        if (typeSyntax is not IdentifierNameSyntax nameSyntax)
            return Enumerable.Empty<Property>();

        var typeSymbol = classModel.GetTypeInfo(nameSyntax).Type;
        var members = typeSymbol?
            .GetMembers()
            .Where(x => x.Name != "EqualityContract")
            .Where(x => x.Kind == SymbolKind.Property);

        if (members is null)
            return Enumerable.Empty<Property>();

        var requestProperties = new List<Property>();

        foreach (var member in members)
        {
            if (member.Kind != SymbolKind.Property)
                continue;

            if (member is not IPropertySymbol propertySymbol)
                continue;

            var attribute = propertySymbol
                .GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.Name is "FromBodyAttribute" or "FromQueryAttribute" or "FromHeaderAttribute" or "FromRouteAttribute")
                ?.AttributeClass?.Name;

            var property = new Property()
            {
                FromAttribute = attribute,
                Name = propertySymbol.Name,
                Type = propertySymbol.Type.ToDisplayString()
            };

            requestProperties.Add(property);
        }
        
        return requestProperties;
    }
    
    private static string GetAssemblyName(Compilation compilation, BaseTypeDeclarationSyntax? typeDeclarationSyntax)
    {
        var assemblyName = typeDeclarationSyntax is not null 
            ? compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree).GetDeclaredSymbol(typeDeclarationSyntax)?.ContainingAssembly.Name 
            : null;

        return assemblyName is null ? string.Empty : $"{assemblyName}.";
    }
}