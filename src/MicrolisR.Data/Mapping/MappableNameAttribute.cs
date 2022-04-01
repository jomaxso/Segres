namespace MicrolisR.Data.Mapping;

[AttributeUsage(AttributeTargets.Property)]
public class MappableNameAttribute : Attribute
{
    public string PropertyName { get; }


    public MappableNameAttribute(string propertyName)
    {
        this.PropertyName = propertyName;
    }
}



