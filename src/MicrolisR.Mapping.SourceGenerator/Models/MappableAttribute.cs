using System.Collections.Generic;

namespace MicrolisR.Mapping.SourceGenerator;

internal class MappableAttribute
{
    public MappableAttribute(string ns, string name, IReadOnlyList<Property> properties)
    {
        Name = name;
        Properties = properties;
        Namespace = ns;
    }

    public string Name { get; }
    public string Namespace { get; }
    public string FullName => $"{Namespace}.{Name}";
    public IReadOnlyList<Property> Properties { get; }
}