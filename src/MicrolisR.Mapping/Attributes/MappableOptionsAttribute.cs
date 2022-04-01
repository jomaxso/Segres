namespace MicrolisR.Mapping.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MappableOptionsAttribute : Attribute
{
    public string? Name { get; }
    public Transform Transform { get; } 
    
    
    public MappableOptionsAttribute(Transform transform)
    {
        Transform = transform;
        this.Name = null;
    }
    
    public MappableOptionsAttribute(Transform transform, string name)
    {
        this.Name = name;
        Transform = transform;
    }
    
    public MappableOptionsAttribute(string name)
    {
        this.Name = name;
        Transform = Transform.Default;
    }
}