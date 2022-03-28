using System;
using System.Collections.Generic;

namespace MicrolisR.Data.SourceGenerator;

internal class MappableClass
{
    public MappableClass(string name, string ns, IReadOnlyList<Property> properties,
        IReadOnlyList<MappableAttributeInfo> mapToClasses)
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

    public IReadOnlyList<MappableAttributeInfo> MapToClasses { get; }
}

internal class Property
{
    public Property(string name, string? mapName)
    {
        Name = name;
   
        MapName = mapName ?? name;
    }
    
    public string Name { get; }
    public string MapName { get; }
    
}

internal class MappableAttributeInfo
{
    public MappableAttributeInfo(string ns, string name, IReadOnlyList<Property> properties)
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