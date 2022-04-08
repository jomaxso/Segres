namespace MicrolisR.Mapping.SourceGenerator.Models;

internal class Property
{
    public Property(string ns, string name, string? mapName, string returnType, bool typeHasMapper = false)
    {
        Namespace = ns;
        Name = name;
        Type = returnType;
        TypeHasMapper = typeHasMapper;
        MapName = mapName ?? name;
    }

    public string Namespace { get; }
    public string Name { get; }

    public string FullTypeName => $"{Namespace}.{Type}";
    public string MapName { get; }
    
    public string Type { get; }
    public bool TypeHasMapper { get; }


}