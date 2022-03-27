using System.Collections.Generic;

namespace MicrolisR.Data.SourceGenerator;

internal class MappableClass
{
    public MappableClass(string name, string ns, IReadOnlyList<Property> properties = null, IReadOnlyList<MappableAttributeInfo> mapToClasses = null)
    {
        Name = name;
        Namespace = ns;
        Properties = properties;
        MapToClasses = mapToClasses;
    }

    public string Name { get;  }
    public string Namespace { get; }
    
    public string FullName => $"{Namespace}.{Name}";
    public string MapperName => FullName.Replace('.', '_');

    public IReadOnlyList<Property> Properties { get; } 

    public IReadOnlyList<MappableAttributeInfo> MapToClasses { get;  } 
}

internal class Property
{
    public Property(string name)
    {
        Name = name;
    }

    public string Name { get;  }
}

internal class MappableAttributeInfo
{
    public MappableAttributeInfo(string name)
    {
        Name = name;
    }

    public string Name { get;  }
}