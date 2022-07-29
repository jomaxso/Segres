using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicrolisRExtension.Endpoint.SourceGeneration;

internal static class Emitter
{
    public static string Emit(IEnumerable<EndpointClass> classes, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return string.Empty;

        var source = new StringBuilder();
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("using MicrolisR;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine();
        source.AppendLine("namespace MicrolisR.G;");
        source.AppendLine();
        
        foreach (var c in classes)
        {
            source.AppendLine($"internal class HttpContextRequestResolver_PrintToConsoleHandler : IHttpContextRequestResolver<{c.RequestClassFullName}, {c.ResponseClassFullName}>");
            source.AppendLine("{");
            source.AppendLine("     private readonly bool _shouldValidate;");
            source.AppendLine($"     public IRequestHandler<{c.RequestClassFullName}, {c.ResponseClassFullName}> RequestHandler {{ get; }}");
            source.AppendLine();
            source.AppendLine($"     public HttpContextRequestResolver_GetUserAuthenticationHandler(IRequestHandler<{c.RequestClassFullName}, {c.ResponseClassFullName}> requestHandler)");
            source.AppendLine("      {");
            source.AppendLine("           RequestHandler = requestHandler;");
            source.AppendLine("           _shouldValidate = ((IHttpContextRequestResolver) this).Validate;");
            source.AppendLine("      };");
            source.AppendLine();
            source.AppendLine("      public Delegate EndpointDelegate =>");
            
            // TODO Customize
            source.AppendLine("           ([FromServices] IMediator mediator, [FromBody] GetUserAuthenticationRequest obj, CancellationToken cancellationToken) =>");
            source.AppendLine("           mediator.SendAsync(obj, _shouldValidate, cancellationToken);");
            
            source.AppendLine("}");
        }
       
        return source.ToString();
    }
}
