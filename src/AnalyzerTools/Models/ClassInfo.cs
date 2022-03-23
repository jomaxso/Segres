using AnalyzerTools.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace AnalyzerTools.Models
{
    public class ClassInfo
    {
        public ClassInfo(string? namespaceName, string accessModifier, string classKeyword, string name,
            List<PropInfo> properties, List<AttributeInfo>? attributes = null)
        {
            this.FullName = $"{namespaceName}.{name}";
            this.Namespace = namespaceName;
            this.Name = name;
            this.Properties = properties;
            this.Keyword = classKeyword;
            this.AccessModifier = accessModifier;
            this.Attributes = attributes ?? new();
        }

        public ClassInfo(ClassInfo classInfo, List<PropInfo> properties)
            : this(classInfo.Namespace, classInfo.AccessModifier, classInfo.Keyword, classInfo.Name, properties, classInfo.Attributes)
        { }

        public ClassInfo(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclaration)
            : this(context.GetClassInfo(classDeclaration) ?? throw new Exception("NULL on creation: ..."))
        { }

        public ClassInfo(ClassInfo classInfo)
            : this(classInfo.Namespace, classInfo.AccessModifier, classInfo.Keyword, classInfo.Name, classInfo.Properties, classInfo.Attributes)
        { }

        public string AccessModifier { get; }
        public string Keyword { get; }
        public string FullName { get; }
        public string? Namespace { get; }
        public string Name { get; }
        public List<PropInfo> Properties { get; }
        public List<AttributeInfo> Attributes { get; }
    }
}