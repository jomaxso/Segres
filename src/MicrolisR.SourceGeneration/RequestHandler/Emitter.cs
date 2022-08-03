using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicrolisR.SourceGeneration.RequestHandler;

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
        source.AppendLine("internal sealed class MicrolisRSourceGenerator_RequestHandlerResolver : IRequestHandlerResolver");
        source.AppendLine("{");
        source.AppendLine("     public Task? ResolveAsync<TResponse>(object handler, IRequestable<TResponse> request, CancellationToken ct)");
        source.AppendLine("     {");
        source.AppendLine("         Task? task = (handler, request) switch");
        source.AppendLine("         {");

        foreach (var c in classes)
        {
            source.AppendLine($"            ({c.HandlerFullName} h, {c.RequestFullName} r) => h.HandleAsync(r, ct),");
        }

        source.AppendLine("             _ => default");
        source.AppendLine("         };");
        source.AppendLine();
        source.AppendLine("         return task;");
        source.AppendLine("     }");
        source.AppendLine("}");
        return source.ToString();
    }
}