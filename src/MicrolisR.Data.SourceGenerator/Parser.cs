using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MicrolisR.Data.SourceGenerator;

internal class Parser
{
    private const string MappableAttribute = "MicrolisR.Data.Mapping.MappableAttribute";

    private readonly Compilation _compilation;
    private readonly Action<Diagnostic> _reportDiagnostic;
    private readonly CancellationToken _cancellationToken;

    public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic,
        CancellationToken contextCancellationToken)
    {
        _compilation = compilation;
        _reportDiagnostic = reportDiagnostic;
        _cancellationToken = contextCancellationToken;
    }

    public IReadOnlyList<MappableClass> GetMappableClasses(IEnumerable<ClassDeclarationSyntax> distinctClasses)
    {
        var list = new List<MappableClass>();

        foreach (var declarationSyntax in distinctClasses)
        {
            var classModel = _compilation.GetSemanticModel(declarationSyntax.SyntaxTree);

            if (_cancellationToken.IsCancellationRequested ||
                classModel.GetDeclaredSymbol(declarationSyntax) is not INamedTypeSymbol symbol)
                return Array.Empty<MappableClass>();

            var className = declarationSyntax.Identifier.Text;
            var classNamespace = symbol.ContainingNamespace
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                .Replace("global::", "");

            var classProperties = GetClassProperties(declarationSyntax, _cancellationToken);
            
            // todo how do i get the namespace
            var mappableAttributeInfo = GetMappableClassAttributeInfos(declarationSyntax, _cancellationToken);
            
            var @class = new MappableClass(className, classNamespace, classProperties, mappableAttributeInfo);
            list.Add(@class);
        }

        return list;
    }

    private static IReadOnlyList<Property> GetClassProperties(TypeDeclarationSyntax declarationSyntax,
        CancellationToken cancellationToken)
    {
        var classProperties = new List<Property>();

        foreach (var member in declarationSyntax.Members)
        {
            if (cancellationToken.IsCancellationRequested)
                return Array.Empty<Property>();

            if (member is PropertyDeclarationSyntax property)
                classProperties.Add(new Property(property.Identifier.Text));
        }

        return classProperties;
    }
    

    private static IReadOnlyList<MappableAttributeInfo> GetMappableClassAttributeInfos(TypeDeclarationSyntax declarationSyntax,
        CancellationToken cancellationToken)
    {
        var mappableAttributeInfo = new List<MappableAttributeInfo>();
        
        foreach (var attributeListSyntax in declarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Array.Empty<MappableAttributeInfo>();
                    
                var expressionSyntax = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.Expression;

                if (expressionSyntax is not TypeOfExpressionSyntax typeOfExpressionSyntax)
                    continue;

                var attributeParameterTypeName = typeOfExpressionSyntax.Type.ToFullString();
                mappableAttributeInfo.Add(new MappableAttributeInfo(attributeParameterTypeName));

            }
        }

        return mappableAttributeInfo;
    }
}