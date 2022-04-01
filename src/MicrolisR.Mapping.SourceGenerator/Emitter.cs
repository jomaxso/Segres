using System.Linq;
using System.Text;
using System.Threading;

namespace MicrolisR.Mapping.SourceGenerator;

internal class Emitter
{
    public string Emit(MappableClass mappableClass, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return string.Empty;

        var source = new StringBuilder();
        AppendMappableClassSource(source, mappableClass);
        return source.ToString();
    }

    private static void AppendMappableClassSource(StringBuilder source, MappableClass mappableClass)
    {
        source.AppendLine($"namespace {mappableClass.Namespace}");
        source.AppendLine("{");
        source.AppendLine();

        foreach (var mapToClass in mappableClass.MapToClasses)
        {
            source.AppendLine($"     partial class {mappableClass.Name} : MicrolisR.Mapping.IMappable<{mapToClass.FullName}>, MicrolisR.Mapping.IMapper<{mappableClass.Name}, {mapToClass.FullName}>");
            source.AppendLine("     {");
            source.AppendLine($"         {mappableClass.Name} MicrolisR.Mapping.IMapper<{mappableClass.Name}, {mapToClass.FullName}>.Map({mapToClass.FullName} target)");
            source.AppendLine("         {");
            source.AppendLine($"             return new {mappableClass.Name}()");
            source.AppendLine("             {");
            
            foreach (var property in mappableClass.Properties)
            {
                if (mapToClass.Properties.Any(p => p.MapName == property.MapName)) 
                    source.AppendLine($"                {property.Name} = target.{property.MapName},");
            }
            
            source.AppendLine("             };");
            source.AppendLine("         }");
            source.AppendLine();
            source.AppendLine($"         {mapToClass.FullName} MicrolisR.Mapping.IMapper<{mappableClass.Name}, {mapToClass.FullName}>.Map({mappableClass.Name} mappable)");
            source.AppendLine("         {");
            source.AppendLine($"             return new {mapToClass.FullName}()");
            source.AppendLine("             {");
            
            foreach (var property in mappableClass.Properties)
            {
                if (mapToClass.Properties.Any(p => p.MapName == property.MapName))
                    source.AppendLine($"                {property.MapName} = mappable.{property.Name},");
            }
            
            source.AppendLine("             };");
            source.AppendLine("         }");
            source.AppendLine("     }");
            source.AppendLine();
        }
        
        source.AppendLine("}");
    }
}