namespace MicrolisR.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MappableAttribute : Attribute
    {
        public MappableAttribute(Type classType)
        {
            this.ClassName = classType.Name;
            this.FullName = classType.FullName;
            this.Namespace = classType.Namespace;
        }

        public string ClassName { get; }
        public string? Namespace { get; }
        public string? FullName { get; }
    }
}
