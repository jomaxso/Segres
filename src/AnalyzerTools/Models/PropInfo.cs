using AnalyzerTools.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace AnalyzerTools.Models
{
    public class PropInfo
    {
        public List<AttributeInfo> Attributes { get; }
        public string Name { get; }
        public string Type { get; }

        public PropInfo(PropInfo propInfo)
        {
            this.Name = propInfo.Name;
            this.Type = propInfo.Type;
            this.Attributes = propInfo.Attributes;
        }

        public PropInfo(string name, string type, List<AttributeInfo>? attributes = null)
        {
            this.Name = name;
            this.Type = type;
            this.Attributes = attributes ?? new();
        }

        public PropInfo(PropertyDeclarationSyntax propertyDeclaration) : this(propertyDeclaration.GetPropInfo())
        {
        }
    }
}