using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MicrolisR.Data.SourceGenerator;

internal class Emitter
{
    public string Emit(IEnumerable<MappableClass> classes, CancellationToken cancellationToken)
    {
        var source = new StringBuilder();

        foreach (var @class in classes)
        {
            if (cancellationToken.IsCancellationRequested)
                return string.Empty;

            AppendMappableClassSource(source, @class);
        }

        return source.ToString();
    }

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
        var mappableClassMapperName = mappableClass.FullName.Replace('.', '_');
        var mappableClassParameterName = $"@{mappableClass.Name.ToLower()}"; 
        
        source.AppendLine("namespace MicrolisR.Data.Mapping");
        source.AppendLine("{");
        source.AppendLine();
        
        foreach (var mapToClass in mappableClass.MapToClasses)
        {
            var mapToClassMapperName = mapToClass.FullName.Replace('.', '_');
            var mapToClassParameterName = $"@{mapToClass.Name.ToLower()}"; 
            
            // MapForward
            source.AppendLine($"     internal static partial class {mappableClassMapperName}");
            source.AppendLine("     {");
            source.AppendLine($"         public static {mappableClass.FullName} MapTo{mappableClass.Name}(this {mapToClass.FullName} {mapToClassParameterName})");
            source.AppendLine("         {");
            source.AppendLine($"             {mapToClassParameterName} = {mapToClassParameterName} ?? throw new ArgumentNullException(nameof({mapToClassParameterName}));");
            source.AppendLine();
            source.AppendLine($"             return new {mappableClass.FullName}()");
            source.AppendLine("             {");

            foreach (var property in mappableClass.Properties)
            {
                if (mapToClass.Properties.Any(p => p.MapName == property.MapName))
                    source.AppendLine($"                  {property.Name} = {mapToClassParameterName}.{property.MapName},");
            }

            source.AppendLine("             };");
            source.AppendLine("         }");
            source.AppendLine("     }");


            source.AppendLine();


            // MapBack
            source.AppendLine($"     internal static partial class {mapToClassMapperName}");
            source.AppendLine("     {");
            source.AppendLine($"         public static {mapToClass.FullName} MapTo{mapToClass.Name}(this {mappableClass.FullName} {mappableClassParameterName})");
            source.AppendLine("         {");
            source.AppendLine($"             {mappableClassParameterName} = {mappableClassParameterName} ?? throw new ArgumentNullException(nameof({mappableClassParameterName}));");
            source.AppendLine();
            source.AppendLine($"             return new {mapToClass.FullName}()");
            source.AppendLine("             {");

            foreach (var property in mappableClass.Properties)
            {
                if (mapToClass.Properties.Any(p => p.MapName == property.MapName))
                    source.AppendLine($"                  {property.MapName} = {mappableClassParameterName}.{property.Name},");
            }

            source.AppendLine("             };");
            source.AppendLine("         }");
            source.AppendLine("     }");

            source.AppendLine();
        }

        source.AppendLine("}");
    }
}