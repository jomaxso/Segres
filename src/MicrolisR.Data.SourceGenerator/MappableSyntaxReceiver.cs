using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MicrolisR.Data.SourceGenerator
{
    public class MappableSyntaxReceiver : ISyntaxReceiver // , ISyntaxContextReceiver
    {
        public List<MappableClass> MappableClasses { get; } = new List<MappableClass>();

        // public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        // {
        //     if (context.Node is ClassDeclarationSyntax classDeclaration is false)
        //         return;
        //     if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol symbol is false)
        //         return;
        //     
        //     var mappableAttribute = classDeclaration.AttributeLists
        //         .SelectMany(x => x.Attributes)
        //         .Select(a => context.SemanticModel.GetTypeInfo(a))
        //         .FirstOrDefault(t => t.Type?.Name == "MappableAttribute");
        //     
        //     var className = classDeclaration.Identifier.Text;
        //     var namespaceDeclaration =
        //         symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        //     
        //     var mappableClass = new MappableClass(namespaceDeclaration, className);
        //     MappableClasses.Add(mappableClass);
        //
        //     var attributeLists =
        //     classDeclaration.AttributeLists.Select(x => x.Attributes.Where(xx => xx.IsKind(SyntaxKind.Attribute)));
        // }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclaration is false)
                return;
            
            var className = classDeclaration.Identifier.Text;
            var namespaceDeclaration = "Demo";
              
            
            var mappableClass = new MappableClass(namespaceDeclaration, className)
            {
                ClassDeclarationSyntax = classDeclaration
            };
            MappableClasses.Add(mappableClass);

            // var attributeLists =
            //     classDeclaration.AttributeLists.Select(x => x.Attributes.Where(xx => xx.IsKind(SyntaxKind.Attribute)));
        }
    }
}