using AnalyzerTools.Helpers;
using AnalyzerTools.Models;
using AnalyzerTools.Receiver;
using MicrolisR.Data.SourceGenerator.MappableInfo;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicrolisR.Data.SourceGenerator
{
    internal static class MappableGeneratorHelper
    {
        internal static StringBuilder GeneratePrivateMappingMethods(this IEnumerable<ClassInfo> infos)
        {
            StringBuilder sourceBuilder = new();

            foreach (var classInfo in infos)
            {
                // var propertyToMap = (classInfo.Attributes.FirstOrDefault() as MappableAttributeInfo);
                // var mapToClassFullName = propertyToMap?.FullName;
                foreach (var mappableClass in
                    classInfo.Attributes.Select(attribute => (attribute as MappableAttributeInfo)))
                {
                    var mapToClassFullName = mappableClass?.FullName;

                    if (mapToClassFullName is null || mappableClass?.FullName is null)
                        continue;

                    sourceBuilder
                        .AddMappingToType(classInfo, mappableClass)
                        .AppendLine()
                        .AddMappingToAttributeType(classInfo, mappableClass);
                }
            }

            return sourceBuilder;
        }

        private static StringBuilder AddMappingToType(this StringBuilder sourceBuilder,
            ClassInfo classInfo, MappableAttributeInfo mappableClass)
        {
            sourceBuilder.Append($@"
        internal static {classInfo.FullName} To{classInfo.Name}(this {mappableClass.FullName} input) => new {classInfo.FullName}()
        {{");
            //return new {classInfo.FullName}();
            foreach (var property in classInfo.Properties)
            {
                var allIgnorable = property.Attributes
                    .Where(a => (a as MappableIgnoreAttributeInfo) is not null)
                    .Select(a => a as MappableIgnoreAttributeInfo)
                    .ToList();

                if (allIgnorable.Any(a => a?.FullName is null))
                    continue;

                if (property.Attributes.Find(a =>
                        (a as MappableIgnoreAttributeInfo)?.FullName?.Equals(mappableClass.FullName) ?? false) is
                    MappableIgnoreAttributeInfo)
                    continue;

                if (property.Attributes.Find(a =>
                        (a as MappableNameAttributeInfo)?.FullName is null) is
                    MappableNameAttributeInfo generalMappableIgnoreAttributeInfo)
                {
                    sourceBuilder.Append(@$"
            {property.Name} = input.{generalMappableIgnoreAttributeInfo.PropertyName},");
                    continue;
                }

                if (property.Attributes.Find(a =>
                        (a as MappableNameAttributeInfo)?.FullName?.Equals(mappableClass.FullName) ?? false) is
                    MappableNameAttributeInfo mappableNameAttributeInfo)
                {
                    sourceBuilder.Append(@$"
            {property.Name} = input.{mappableNameAttributeInfo.PropertyName},");
                    continue;
                }

                if (mappableClass.PropertyNames.Contains(property.Name))
                {
                    sourceBuilder.Append($@"
            {property.Name} = input.{property.Name},");
                }
            }

            sourceBuilder.Append($@"
        }};");

            return sourceBuilder;
        }

        private static void AddMappingToAttributeType(this StringBuilder sourceBuilder,
            ClassInfo classInfo, MappableAttributeInfo mappableClass)
        {
            sourceBuilder.Append($@"
        internal static {mappableClass.FullName} To{mappableClass.ClassName}(this {classInfo.FullName} input) => new {mappableClass.FullName}()
        {{");
            foreach (var property in classInfo.Properties)
            {
                var allIgnorable = property.Attributes
                    .Where(a => (a as MappableIgnoreAttributeInfo) is not null)
                    .Select(a => a as MappableIgnoreAttributeInfo)
                    .ToList();

                if (allIgnorable.Any(a => a?.ClassName is null))
                    continue;

                if (property.Attributes.Find(a =>
                        (a as MappableIgnoreAttributeInfo)?.FullName?.Equals(mappableClass.FullName) ?? false) is
                    MappableIgnoreAttributeInfo)
                    continue;

                if (property.Attributes.Find(a =>
                        (a as MappableNameAttributeInfo)?.FullName is null) is
                    MappableNameAttributeInfo generalMappableIgnoreAttributeInfo)
                {
                    sourceBuilder.Append(@$"
            {generalMappableIgnoreAttributeInfo.PropertyName} = input.{property.Name},");
                    continue;
                }

                if (property.Attributes.Find(a =>
                        ((a as MappableNameAttributeInfo)?.FullName?.Equals(mappableClass.FullName) ?? false)) is
                    MappableNameAttributeInfo mappableNameAttributeInfo)
                {
                    sourceBuilder.Append(@$"
            {mappableNameAttributeInfo.PropertyName} = input.{property.Name},");
                    continue;
                }

                if (mappableClass.PropertyNames.Contains(property.Name))
                {
                    sourceBuilder.Append($@"
            {property.Name} = input.{property.Name},");
                }
            }

            sourceBuilder.Append($@"
        }};");
        }

        internal static IEnumerable<ClassInfo> GetMappableClassInfos(this GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver receiver)
                return Array.Empty<ClassInfo>();

            var mappableAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("Simplelize.Mapping.Attributes.MappableAttribute")!;

            List<ClassInfo> outputList = new();

            foreach (var classDeclaration in receiver.Candidates)
            {
                var classModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                ITypeSymbol? classSymbol = (ITypeSymbol?)classModel.GetDeclaredSymbol(classDeclaration);

                if (classSymbol is null)
                    continue;

                // Do not generate an overload if we do not find our attribute on the class in question
                if (!classSymbol.GetAttributes().Any(a =>
                        a.AttributeClass!.Equals(mappableAttributeSymbol, SymbolEqualityComparer.Default)))
                    continue;

                // Get the Information of the Class witch uses the [MappableAttribute]
                var classInfo = context.GetClassInfo(classDeclaration);

                if (classInfo is null || outputList.Any(c => c.FullName.Equals(classInfo.FullName)))
                    continue;

                AddMappableAttributeInfos(classSymbol, classInfo.Attributes, context.Compilation);
                AddPropertyInfos(classDeclaration, classModel, classInfo);

                outputList.Add(classInfo);
            }

            return outputList;
        }

        private static void AddPropertyInfos(SyntaxNode classDeclaration, SemanticModel classModel, ClassInfo classInfo)
        {
            var propertyDeclarations = classDeclaration
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>();

            foreach (var propertyDeclaration in propertyDeclarations)
            {
                IPropertySymbol? propertySymbol = (IPropertySymbol?)classModel.GetDeclaredSymbol(propertyDeclaration);

                if (propertySymbol is null)
                    continue;

                var propertyIndex = classInfo.Properties
                    .FindIndex(p => p.Name.Equals(propertyDeclaration.Identifier.Text));

                AddMappableNameAttributeInfos(propertySymbol, classInfo.Properties[propertyIndex]);
                AddMappableIgnoreAttributeInfos(propertySymbol, classInfo.Properties[propertyIndex]);
            }
        }

        private static void AddMappableAttributeInfos(ITypeSymbol classSymbol,
            ICollection<AttributeInfo> attributeInfos, Compilation compilation)
        {
            var attributes = classSymbol
                .GetAttributes()
                .Where(a => a.AttributeClass?.Name is "MappableAttribute");

            foreach (var attribute in attributes)
            {
                try
                {
                    var attributeClassObjectType = attribute.ConstructorArguments[0].Value;
                    var attributeClassSymbol =
                        compilation.GetTypeByMetadataName(attributeClassObjectType?.ToString() ?? string.Empty);

                    if (attributeClassSymbol is null)
                        continue;

                    var fullName = attributeClassSymbol.ToString();
                    var className = attributeClassSymbol.Name;
                    var namespaceName = attributeClassSymbol.ContainingNamespace.ToString();
                    //var namespaceName = fullName.Remove(fullName.Length - className.Length - 1, className.Length + 1);

                    if (attributeInfos.Any(a => fullName!.Equals((a as MappableAttributeInfo)?.FullName)))
                        continue;

                    attributeInfos.Add(new MappableAttributeInfo(className, namespaceName, fullName,
                        attributeClassSymbol.MemberNames));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static void AddMappableNameAttributeInfos(IPropertySymbol propertySymbol, PropInfo propInfo)
        {
            var attributes = propertySymbol
                .GetAttributes()
                .Where(a => a.AttributeClass?.Name is "MappableNameAttribute");

            foreach (var attribute in attributes)
            {
                try
                {
                    var propertyName = attribute.ConstructorArguments[0].Value?.ToString();
                    var fullName = attribute.ConstructorArguments[1].Value?.ToString();

                    if (fullName is null)
                    {
                        propInfo.Attributes.Add(new MappableNameAttributeInfo(propertyName: propertyName));
                        return;
                    }

                    var name = fullName.Split('.').Last();
                    var ns = fullName.Remove(fullName.Length - name.Length - 1, name.Length + 1);
                    propInfo.Attributes.Add(new MappableNameAttributeInfo(name, ns, fullName, propertyName));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static void AddMappableIgnoreAttributeInfos(ISymbol propertySymbol, PropInfo propInfo)
        {
            var attributes = propertySymbol
                .GetAttributes()
                .Where(a => a.AttributeClass?.Name is "MappableIgnoreAttribute");

            foreach (var attribute in attributes)
            {
                try
                {
                    var propertyName = propertySymbol.Name;
                    var fullName = attribute.ConstructorArguments[0].Value?.ToString();

                    if (fullName is null)
                    {
                        propInfo.Attributes.Add(new MappableIgnoreAttributeInfo(propertyName: propertyName));
                        return;
                    }

                    var name = fullName.Split('.').Last();
                    var ns = fullName.Remove(fullName.Length - name.Length - 1, name.Length + 1);
                    propInfo.Attributes.Add(new MappableIgnoreAttributeInfo(name, ns, fullName, propertyName));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
