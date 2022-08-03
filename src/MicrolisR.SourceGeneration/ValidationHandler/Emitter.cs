using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicrolisR.SourceGeneration.ValidationHandler;

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
        source.AppendLine("internal record struct MicrolisRSourceGenerator_ValidationHandlerResolver : IValidationHandlerResolver");
        source.AppendLine("{");
        source.AppendLine("     public void Resolve(object handler, IValidatable value)");
        source.AppendLine("     {");
        source.AppendLine("         switch (handler, value)");
        source.AppendLine("         {");

        foreach (var c in classes)
        { 
            source.AppendLine($"            case ({c.HandlerFullName} v, {c.RequestFullName} r):");
            source.AppendLine($"                v.Validate(r);");
            source.AppendLine($"                break;");
        }
        
        source.AppendLine("         };");
        source.AppendLine("     }");
        source.AppendLine("}");
        return source.ToString();
    }
}