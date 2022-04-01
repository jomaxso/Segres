namespace MicrolisR.Mapping.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MappableAttribute : Attribute
{
    public MappableAttribute(Type classType)
    {
        this.ClassType = classType;
    }

    public Type ClassType { get; }
}