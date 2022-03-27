using System.Collections.Generic;
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
        source.AppendLine("namespace MicrolisR.Data.Mapping");
        source.AppendLine("{");
        source.AppendLine($"     internal static partial class {mappableClass.MapperName}");
        source.AppendLine("     {");
        source.AppendLine($"         public static {mappableClass.FullName} To{mappableClass.Name}(this Demo.Class2 class2)");
        source.AppendLine("         {");
        source.AppendLine($"             return new {mappableClass.FullName}()");
        source.AppendLine("             {");
        
        foreach (var mapToClass in mappableClass.MapToClasses)
        {
            source.AppendLine($"                   // {mapToClass.Name}");
        }
        
        foreach (var property in mappableClass.Properties)
        {
            source.AppendLine($"                  {property.Name} = class2.{property.Name},");
        }
        
        source.AppendLine("             };");
        source.AppendLine("         }");
        source.AppendLine("     }");
        source.AppendLine("}");
    }
}