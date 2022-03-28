using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace MicrolisR.Data.SourceGenerator;

internal class Parser
{
    private const string MappableIgnoreAttribute = "MicrolisR.Data.Mapping.MappableIgnoreAttribute";
    private const string MappableNameAttribute = "MicrolisR.Data.Mapping.MappableNameAttribute";

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
                ModelExtensions.GetDeclaredSymbol(classModel, declarationSyntax) is not INamedTypeSymbol symbol)
                return Array.Empty<MappableClass>();

            var className = declarationSyntax.Identifier.Text;
            var classNamespace = symbol.ContainingNamespace.Name;

            var classProperties = GetClassProperties(declarationSyntax, _cancellationToken);
            var mappableAttributeInfo = GetMappableClassAttributeInfos(declarationSyntax, _cancellationToken);

            var @class = new MappableClass(className, classNamespace, classProperties, mappableAttributeInfo);
            list.Add(@class);
        }

        return list;
    }

    private IReadOnlyList<Property> GetClassProperties(TypeDeclarationSyntax declarationSyntax,
        CancellationToken cancellationToken)
    {
        var classProperties = new List<Property>();

        foreach (var member in declarationSyntax.Members)
        {
            if (cancellationToken.IsCancellationRequested)
                return Array.Empty<Property>();

            if (member is not PropertyDeclarationSyntax property)
                continue;

            // check for MappableIgnore by Type
            var mappableIgnoreAttributes = GetAttributes(property, MappableIgnoreAttribute);
            if (mappableIgnoreAttributes.Any())
                continue;
            
            // check for MappableName

            var mappableNameAttributes = GetAttributes(property, MappableNameAttribute).FirstOrDefault();

            string? mapName = null;
            var mapToPropertyNameExpression = mappableNameAttributes?.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
            switch (mapToPropertyNameExpression)
            {
                case LiteralExpressionSyntax stringArgument:
                    mapName = stringArgument.Token.ValueText;
                    break;
                case InvocationExpressionSyntax invocationExpressionSyntax:
                    var argument = invocationExpressionSyntax.ArgumentList.Arguments.FirstOrDefault();
                    var simpleMemberExpression = argument?.Expression as MemberAccessExpressionSyntax;
                    mapName = simpleMemberExpression?.Name.Identifier.Text;
                    break;
                default:
                    mapName = null;
                    break;
            }

            // string? targetTypeFullName = null;
            // var mapToPropertyClassTypeExpression =  mappableNameAttributes?.ArgumentList?.Arguments[1]?.Expression;
            // switch (mapToPropertyClassTypeExpression)
            // {
            //     case TypeOfExpressionSyntax typeOfExpressionSyntax:
            //         var targetClassSymbol = ModelExtensions.GetTypeInfo(_compilation.GetSemanticModel(typeOfExpressionSyntax.SyntaxTree),
            //             typeOfExpressionSyntax.Type).Type as INamedTypeSymbol;
            //         targetTypeFullName = targetClassSymbol?.ToDisplayString();
            //         break;
            // }

            classProperties.Add(new Property(property.Identifier.Text, mapName));
        }

        return classProperties;
    }

    private IReadOnlyList<AttributeSyntax> GetAttributes(MemberDeclarationSyntax member,
        in string containingAttribute)
    {
        List<AttributeSyntax> attributes = new();

        foreach (var attributeList in member.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (ModelExtensions.GetSymbolInfo(_compilation.GetSemanticModel(member.SyntaxTree), attribute,
                            _cancellationToken)
                        .Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName == containingAttribute)
                    attributes.Add(attribute);
            }
        }

        return attributes;
    }

    private IReadOnlyList<MappableAttributeInfo> GetMappableClassAttributeInfos(TypeDeclarationSyntax declarationSyntax,
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

                if (expressionSyntax is not TypeOfExpressionSyntax typeOfExpressionSyntax ||
                    ModelExtensions.GetTypeInfo(_compilation.GetSemanticModel(typeOfExpressionSyntax.SyntaxTree),
                        typeOfExpressionSyntax.Type).Type is not INamedTypeSymbol targetClass)
                    continue;

                var name = targetClass.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                var ns = targetClass.ContainingNamespace.Name;
                var properties = GetTargetClassProperties(targetClass, cancellationToken);

                mappableAttributeInfo.Add(new MappableAttributeInfo(ns, name, properties));
            }
        }

        return mappableAttributeInfo;
    }

    private IReadOnlyList<Property> GetTargetClassProperties(INamedTypeSymbol targetClass,
        CancellationToken cancellationToken)
    {
        var classProperties = new List<Property>();


        foreach (var member in targetClass.GetMembers())
        {
            if (member is not IPropertySymbol property)
                continue;

            if (cancellationToken.IsCancellationRequested)
                return Array.Empty<Property>();

            classProperties.Add(new Property(property.Name, property.Name));
        }

        return classProperties;
    }
}