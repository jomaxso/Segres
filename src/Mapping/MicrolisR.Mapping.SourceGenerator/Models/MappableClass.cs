using System.Collections.Generic;

namespace MicrolisR.Mapping.SourceGenerator.Models;



internal class MappableClass
{
    public MappableClass(string name, string ns, IReadOnlyList<Property> properties,
        IReadOnlyList<MappableAttribute> mapToClasses)
    {
        Name = name;
        Namespace = ns;
        Properties = properties;
        MapToClasses = mapToClasses;
    }

    public string Name { get; }
    public string Namespace { get; }

    public string FullName => $"{Namespace}.{Name}";
   

    public IReadOnlyList<Property> Properties { get; }

    public IReadOnlyList<MappableAttribute> MapToClasses { get; }
}