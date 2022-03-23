using AnalyzerTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicrolisR.Data.SourceGenerator.MappableInfo
{
    internal class MappableAttributeInfo : AttributeInfo
    {
        public MappableAttributeInfo(string className, string? namespaceName, string? fullName, IEnumerable<string>? propertyNames)
        {
            ClassName = className;
            Namespace = namespaceName;
            FullName = fullName;
            PropertyNames = propertyNames?.ToList() ?? new();
        }

        public string ClassName { get; }
        public string? Namespace { get; }
        public string? FullName { get; }
        public List<string> PropertyNames { get; }
    }
}