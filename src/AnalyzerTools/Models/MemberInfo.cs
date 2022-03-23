namespace AnalyzerTools.Models
{
    public class MemberInfo
    {
        public MemberInfo(string name, string? value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }
        public string? Value { get; }


    }
}