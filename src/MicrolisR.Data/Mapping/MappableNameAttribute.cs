namespace MicrolisR.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MappableNameAttribute : Attribute
    {
        public string PropertyName { get; }
        public Type? ClassName { get; }

        public MappableNameAttribute(string propertyName, Type? type = null)
        {
            this.PropertyName = propertyName;
            this.ClassName = type;
        }
    }
}
