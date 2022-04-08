namespace MicrolisR.Mapping.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MappableOptionsAttribute : Attribute
{
    public string? Name { get; }

    public MappableOptionsAttribute(string name)
    {
        this.Name = name;
    }
}