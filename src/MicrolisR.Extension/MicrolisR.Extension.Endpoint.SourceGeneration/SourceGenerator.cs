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

            var requestFullNames = GetRequestClassFullName(compilation, handlerDeclarationSyntax);
            var handlerFullName = GetHandlerClassFullName(compilation, handlerDeclarationSyntax);

            if (handlerFullName is null)
                continue;

            var requestHandlerClasses = requestFullNames
                .Where(x => x is not null)
                .Select(requestFullName => new EndpointClass(handlerFullName, requestFullName));

            targetClasses.AddRange(requestHandlerClasses);
        }
        
        return targetClasses;
    }

    private static IEnumerable<string> GetRequestClassFullName(Compilation compilation, BaseTypeDeclarationSyntax handlerDeclarationSyntax)
    {
        var requestDeclarationSyntaxList = handlerDeclarationSyntax
            .BaseList?.Types
            .Select(x => x.Type)
            .OfType<GenericNameSyntax>()
            .Where(x => x.Identifier.Text == "IRequestHandler")?
            .Select(x => x.TypeArgumentList.Arguments.FirstOrDefault()).Where(x => x is not null)
            .OfType<IdentifierNameSyntax>();

        if (requestDeclarationSyntaxList is null)
            return Enumerable.Empty<string>();

        var classModel = compilation.GetSemanticModel(handlerDeclarationSyntax.SyntaxTree);

        var requestFullNames = new List<string>();

        foreach (var requestDeclarationSyntax in requestDeclarationSyntaxList)
        {
            var requestType = classModel.GetTypeInfo(requestDeclarationSyntax).Type;

            if (requestType is null)
                continue;
            
            requestFullNames.Add($"{requestType.ContainingNamespace.ToDisplayString()}.{requestType.Name}");
        }

        return requestFullNames;
    }

    private static string? GetHandlerClassFullName(Compilation compilation, BaseTypeDeclarationSyntax declarationSyntax)
    {
        var classModel = compilation.GetSemanticModel(declarationSyntax.SyntaxTree);

        if (classModel.GetDeclaredSymbol(declarationSyntax) is not INamedTypeSymbol symbol)
            return null;

        var className = declarationSyntax.Identifier.Text;
        var classNamespace = symbol.ContainingNamespace.ToDisplayString();
        return $"{classNamespace}.{className}";
    }
}