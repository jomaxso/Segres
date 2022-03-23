using AnalyzerTools.Models;

namespace MicrolisR.Data.SourceGenerator.MappableInfo
{
    internal class MappableIgnoreAttributeInfo : AttributeInfo
    {
        public MappableIgnoreAttributeInfo(string? className = null, string? namespaceName = null, string? fullName = null, string? propertyName = null)
        {
            ClassName = className;
            Namespace = namespaceName;
            FullName = fullName;
            PropertyName = propertyName;
        }

        public string? Namespace { get; }
        public string? ClassName { get; }
        public string? FullName { get; }
        public string? PropertyName { get; }
    }
}