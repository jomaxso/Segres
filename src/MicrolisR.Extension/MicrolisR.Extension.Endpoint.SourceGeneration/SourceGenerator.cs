using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        var result = Emitter.Emit(targetClasses, context.CancellationToken);
        context.AddSource($"AssemblyName.HttpContextRequestResolvers.g.cs", SourceText.From(result, Encoding.UTF8));
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

    private static IEnumerable<Property> GetRequestProperties(Compilation compilation, IEnumerable<GenericNameSyntax>? requestDeclarationSyntaxes)
    {
        if (requestDeclarationSyntaxes is null)
            return Enumerable.Empty<Property>();

        var requestDeclarationSyntaxList = requestDeclarationSyntaxes
            .Select(x => x.TypeArgumentList.Arguments.FirstOrDefault()).Where(x => x is not null)
            .OfType<IdentifierNameSyntax>();

        var properties = new List<Property>();

        foreach (var requestDeclarationSyntax in requestDeclarationSyntaxList)
        {
            throw new Exception(JsonSerializer.Serialize(requestDeclarationSyntax.Identifier.Text));
        }

        return properties;
    }

    private static IEnumerable<EndpointClass> GetEndpointClasses(Compilation compilation, BaseTypeDeclarationSyntax handlerDeclarationSyntax)
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
            // REQUEST
            if (syntax.TypeArgumentList.Arguments.FirstOrDefault() is not IdentifierNameSyntax requestNameSyntax)
                continue;
            
            var requestType = classModel.GetTypeInfo(requestNameSyntax).Type;

            if (requestType is null)
                continue;

            var requestFullName = $"{requestType.ContainingNamespace.ToDisplayString()}.{requestType.Name}";
            
            // RESPONSE
            if (syntax.TypeArgumentList.Arguments[1] is not IdentifierNameSyntax responseNameSyntax)
                continue;
            
            var responseType = classModel.GetTypeInfo(responseNameSyntax).Type;

            if (responseType is null)
                continue;

            var responseFullName = $"{responseType.ContainingNamespace.ToDisplayString()}.{responseType.Name}";
            
            // PROPERTIES
            
            var requestClassModel = compilation.GetSemanticModel(handlerDeclarationSyntax.SyntaxTree);
            
            var requestProperties = new List<Property>();
            foreach (PropertyDeclarationSyntax declarationSyntax in Array.Empty<PropertyDeclarationSyntax>())
            {
                var property = new Property() {FromAttribute = "FromRoute", Name = "Value", Type = "int"};
                requestProperties.Add(property);
            }

            endpointClasses.Add(new EndpointClass(requestFullName, responseFullName, requestProperties));
        }

        return endpointClasses;
    }
}