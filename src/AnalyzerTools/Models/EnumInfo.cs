using System.Collections.Generic;

namespace AnalyzerTools.Models
{
    public class EnumInfo
    {
        public EnumInfo(string? ns, string accessModifier, string keyword, string name, string type,
            IEnumerable<MemberInfo> members)
        {
            this.Namespace = ns;
            this.AccessModifier = accessModifier;
            this.Keyword = keyword;
            this.Name = name;
            this.Members = members;
            this.Type = type;
            this.FullName = $"{ns}.{name}";
        }

        public IEnumerable<MemberInfo> Members { get; }

        public string? Namespace { get; }

        public string Name { get; }

        public string FullName { get; }

        public string AccessModifier { get; }

        public string Keyword { get; }

        public string Type { get; }
    }
}