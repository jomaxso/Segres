namespace MicrolisR.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MappableIgnoreAttribute : Attribute
    {
        public MappableIgnoreAttribute(Type? type = null)
        {
            this.ClassName = type;
        }

        public Type? ClassName { get; }
    }
}
