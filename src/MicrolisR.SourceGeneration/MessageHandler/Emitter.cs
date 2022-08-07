using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicrolisR.SourceGeneration.MessageHandler;

internal static class Emitter
{
    public static string Emit(IEnumerable<string> classes, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return string.Empty;

        var source = new StringBuilder();
        
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using MicrolisR;");
        source.AppendLine("using System.Threading.Tasks;");
        source.AppendLine();
        source.AppendLine($"namespace MicrolisR.G;");
        source.AppendLine();
        source.AppendLine("internal sealed class MicrolisRSourceGenerator_MessageHandlerResolver : IMessageHandlerResolver");
        source.AppendLine("{");
        source.AppendLine("     public Task ResolveAsync(object handler, IMessage message, CancellationToken ct)");
        source.AppendLine("     {");
        source.AppendLine("         return (handler, message) switch");
        source.AppendLine("         {");

        foreach (var c in classes)
        {
            source.AppendLine($"            (IMessageHandler<{c}> h, {c} m) => h.SubscribeAsync(m, ct),");
        }

        source.AppendLine("             _ => Task.CompletedTask");
        source.AppendLine("         };");
        source.AppendLine("     }");
        source.AppendLine("}");
        
        return source.ToString();
    }
}
