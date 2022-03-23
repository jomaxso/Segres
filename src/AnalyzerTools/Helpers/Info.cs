using AnalyzerTools.Models;
using AnalyzerTools.Receiver;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalyzerTools.Helpers
{
    public static class Info
    {
        internal static IEnumerable<ClassInfo> GetClassInfosByAttribute(this GeneratorExecutionContext context,
            string typeMetadataName)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver receiver)
                return Array.Empty<ClassInfo>();

            var attributeSymbol = context.Compilation.GetTypeByMetadataName(typeMetadataName);

            List<ClassInfo> outputList = new();

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var classModel = context.Compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
                var classSymbol = classModel.GetDeclaredSymbol(typeDeclaration);

                if (classSymbol is null)
                    continue;

                // Do not generate an overload if we do not find our attribute on the class in question
                if (!classSymbol.GetAttributes().Any(a =>
                        a.AttributeClass!.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                    continue;

                // Get the Information of the Class witch uses the [MappableAttribute]
                var classInfo = context.GetClassInfo(typeDeclaration);

                if (classInfo is not null && !outputList.Any(c => c.FullName.Equals(classInfo.FullName)))
                    outputList.Add(classInfo);
            }

            return outputList;
        }

        /// <summary>
        /// Get the Information of the Class
        /// </summary>
        /// <param name="context"></param>
        /// <returns>the information of the class</returns>
        public static IEnumerable<ClassInfo> GetClassInfos<TReceiver, TSyntax>(this GeneratorExecutionContext context)
            where TReceiver : SyntaxReceiver<TSyntax>
            where TSyntax : BaseTypeDeclarationSyntax
        {
            if (context.SyntaxReceiver is TReceiver receiver is false)
                return new List<ClassInfo>();

            List<ClassInfo> outputList = new();

            foreach (var typeDeclaration in receiver.Candidates)
            {
                if (typeDeclaration is not ClassDeclarationSyntax classDeclaration)
                    continue;

                var classInfo = context.GetClassInfo(classDeclaration);

                if (classInfo is not null)
                    outputList.Add(classInfo);
            }

            return outputList;
        }


        /// <summary>
        /// Get the Information of the Class
        /// </summary>
        /// <param name="context"></param>
        /// <returns>the information of the class</returns>
        public static List<ClassInfo> GetClassInfos(this GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is ClassSyntaxReceiver receiver is false)
                return new();

            List<ClassInfo> outputList = new();

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var classInfo = context.GetClassInfo(typeDeclaration);

                if (classInfo is not null)
                    outputList.Add(classInfo);
            }

            return outputList;
        }


        public static ClassInfo? GetClassInfo(this GeneratorExecutionContext context,
            ClassDeclarationSyntax typeDeclaration)
        {
            var symbol = context.Compilation
                .GetSemanticModel(typeDeclaration.SyntaxTree)
                .GetDeclaredSymbol(typeDeclaration);

            if (symbol is null)
                return null;

            var classProperties = typeDeclaration.GetPropInfos();
            var classNamespace = symbol.ContainingNamespace.ToString();
            var className = typeDeclaration.Identifier.Text;
            var classKeyword = typeDeclaration.Keyword.Text;
            var classAccessModifier = typeDeclaration.Modifiers.ToString();

            return new(classNamespace, classAccessModifier, classKeyword, className, classProperties);
        }

        /// <summary>
        /// Get the Information of the Class
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetEnumInfos(this GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is EnumSyntaxReceiver receiver is false)
                return new();

            List<EnumInfo> outputList = new();

            foreach (var typeDeclaration in receiver.Candidates)
            {
                var enumInfo = context.GetEnumInfo(typeDeclaration);

                if (enumInfo is null) continue;

                outputList.Add(enumInfo);
            }

            return outputList;
        }


        /// <summary>
        /// Get the Information of the Class
        /// </summary>
        /// <returns>the information of the class</returns>
        public static EnumInfo? GetEnumInfo(this GeneratorExecutionContext context, EnumDeclarationSyntax enumDeclaration)
        {
            var enumSymbol = context.Compilation
                .GetSemanticModel(enumDeclaration.SyntaxTree)
                .GetDeclaredSymbol(enumDeclaration);

            if (enumSymbol is null)
                return null;

            var enumNamespace = enumSymbol.ContainingNamespace.ToString();
            var enumName = enumDeclaration.Identifier.Text;
            var enumKeyword = enumDeclaration.EnumKeyword.Text;
            var enumAccessModifier = enumDeclaration.Modifiers.ToString();
            var type = "int"; // TODO Welche werte werden angegeben

            List<MemberInfo> enumMembers = new();

            foreach (var enumMember in enumDeclaration.Members)
            {
                var enumNameArgumentList = enumMember.AttributeLists
                    .SingleOrDefault(x => x.ToString().StartsWith("[EnumName"))?
                    .Attributes.SingleOrDefault()?.ArgumentList; // Todo Als übergabewert einbinden

                var alternativeName = enumNameArgumentList?.Arguments.Single().ToString().Trim('"');
                enumMembers.Add(new(enumMember.Identifier.Text, alternativeName));
            }

            return new(enumNamespace, enumAccessModifier, enumKeyword, enumName, type, enumMembers);
        }

        public static List<PropInfo> GetPropInfos(this TypeDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .OfType<PropertyDeclarationSyntax>()
                .Select(GetPropInfo)
                .ToList();
        }

        public static PropInfo GetPropInfo(this PropertyDeclarationSyntax propertyDeclaration)
        {
            var propertyType = propertyDeclaration.Type.ToFullString();
            var propertyName = propertyDeclaration.Identifier.Text;

            return new(propertyName, propertyType);
        }
    }
}