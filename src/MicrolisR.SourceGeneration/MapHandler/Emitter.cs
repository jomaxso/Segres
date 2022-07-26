using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicrolisR.SourceGeneration.MapHandler;

internal static class Emitter
{
    public static string Emit(IEnumerable<HandlerClass> classes, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return string.Empty;

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using MicrolisR;");
        source.AppendLine($"namespace MicrolisR.G;");
        source.AppendLine();
        source.AppendLine("internal record struct MicrolisRSourceGenerator_MapHandlerResolver : IMapHandlerResolver");
        source.AppendLine("{");
        source.AppendLine("     public T? Resolve<T>(object handler, IMappable<T> value)");
        source.AppendLine("     {");
        source.AppendLine("         return (handler, value) switch");
        source.AppendLine("         {");

        foreach (var c in classes)
        { 
           source.AppendLine($"            ({c.HandlerFullName} m, {c.RequestFullName} r) => m.Map(r) is T result ? result : default,");
        }

        source.AppendLine("             _ => default");
        source.AppendLine("         };");
        source.AppendLine("     }");
        source.AppendLine("}");
        return source.ToString();
    }
}
