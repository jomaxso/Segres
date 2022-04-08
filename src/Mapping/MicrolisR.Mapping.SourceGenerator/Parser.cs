using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MicrolisR.Mapping.SourceGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MicrolisR.Mapping.SourceGenerator;

internal class Parser
{
    private const string MappableIgnoreAttribute = "MicrolisR.Mapping.Attributes.MappableIgnoreAttribute";
    private const string MappableOptionsAttribute = "MicrolisR.Mapping.Attributes.MappableOptionsAttribute";
    private const string MappableAttribute = "MicrolisR.Mapping.Attributes.MappableAttribute";

    private readonly Compilation _compilation;
    private readonly Action<Diagnostic> _diagnostic;
    private readonly CancellationToken _cancellationToken;

    public Parser(Compilation compilation, Action<Diagnostic> diagnostic, CancellationToken cancellationToken)
    {
        _compilation = compilation;
        _diagnostic = diagnostic;
        _cancellationToken = cancellationToken;
    }

    public IReadOnlyList<MappableClass> GetMappableClasses(IEnumerable<TypeDeclarationSyntax> classes)
    {
        var list = new List<MappableClass>();

        foreach (var declarationSyntax in classes)
        {
            var classModel = _compilation.GetSemanticModel(declarationSyntax.SyntaxTree);

            if (_cancellationToken.IsCancellationRequested ||
                ModelExtensions.GetDeclaredSymbol(classModel, declarationSyntax) is not INamedTypeSymbol symbol)
                return Array.Empty<MappableClass>();

            var className = declarationSyntax.Identifier.Text;
            var classNamespace = symbol.ContainingNamespace.ToDisplayString();
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

            var mappableOptionsAttributes = GetAttributes(property, MappableOptionsAttribute).FirstOrDefault();

            var expression = mappableOptionsAttributes?.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
            var mapName = GetMapName(expression, property);

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

            // todo check property if it has any mapper ? YES: HasMapper = true, : NO: HasMapper = false;

            var targetClass =
                _compilation.GetSemanticModel(property.Type.SyntaxTree).GetTypeInfo(property.Type).Type as
                    INamedTypeSymbol;
            
            var hasMapper = targetClass?.GetAttributes()
                .Any(x => x.AttributeClass?.ToDisplayString() == MappableAttribute) ?? false;

            var newProperty = new Property(targetClass?.ContainingNamespace.Name ?? "",property.Identifier.Text, mapName, property.Type.ToString(), hasMapper);
            classProperties.Add(newProperty);
        }

        return classProperties;
    }

    private static string? GetMapName(ExpressionSyntax? expressionSyntax, PropertyDeclarationSyntax property)
    {
        switch (expressionSyntax)
        {
            case LiteralExpressionSyntax stringArgument:
                return stringArgument.Token.ValueText;

            case InvocationExpressionSyntax invocationExpressionSyntax:
                var argument = invocationExpressionSyntax.ArgumentList.Arguments.FirstOrDefault();
                var simpleMemberExpression = argument?.Expression as MemberAccessExpressionSyntax;
                return simpleMemberExpression?.Name.Identifier.Text;

            default:
                return property.Identifier.Text;
        }
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

    private IReadOnlyList<MappableAttribute> GetMappableClassAttributeInfos(TypeDeclarationSyntax declarationSyntax,
        CancellationToken cancellationToken)
    {
        var mappableAttributeInfo = new List<MappableAttribute>();

        foreach (var attributeListSyntax in declarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Array.Empty<MappableAttribute>();

                var expressionSyntax = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.Expression;

                if (expressionSyntax is not TypeOfExpressionSyntax typeOfExpressionSyntax ||
                    ModelExtensions.GetTypeInfo(_compilation.GetSemanticModel(typeOfExpressionSyntax.SyntaxTree),
                        typeOfExpressionSyntax.Type).Type is not INamedTypeSymbol targetClass)
                    continue;

                var name = targetClass.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                var ns = targetClass.ContainingNamespace.Name;
                var properties = GetTargetClassProperties(targetClass, cancellationToken);

                mappableAttributeInfo.Add(new MappableAttribute(ns, name, properties));
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

            classProperties.Add(
                new Property(property.Type.ContainingNamespace.Name, property.Name, property.Name, property.Type.Name));
        }

        return classProperties;
    }
}