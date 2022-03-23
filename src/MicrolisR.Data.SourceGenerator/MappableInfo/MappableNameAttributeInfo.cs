using AnalyzerTools.Models;

namespace MicrolisR.Data.SourceGenerator.MappableInfo
{
    internal class MappableNameAttributeInfo : AttributeInfo
    {
        public MappableNameAttributeInfo(string? className = null, string? namespaceName = null, string? fullName = null, string? propertyName = null)
        {
            ClassName = className;
            Namespace = namespaceName;
            FullName = fullName;
            PropertyName = propertyName;

        }

        public string? ClassName { get; }
        public string? Namespace { get; }
        public string? FullName { get; }
        public string? PropertyName { get; }
    }

}
