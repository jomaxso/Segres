namespace MicrolisR.Mapping.SourceGenerator;

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