using System.Collections.Generic;
using System.Linq;
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
        source.AppendLine("using System;");
        source.AppendLine("using System.Threading;");
        source.AppendLine("using System.Threading.Tasks;");
        source.AppendLine("using MicrolisR;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine();
        source.AppendLine("namespace MicrolisR.G;");
        source.AppendLine();
        
        foreach (var c in classes)
        {
            var delegateBody = DelegateBody(c);
            
            source.AppendLine($"internal class HttpContextRequestResolver_{c.RequestAsName}_{c.ResponseAsName} : IHttpContextRequestResolver<{c.RequestClassFullName}, {c.ResponseClassFullName}>");
            source.AppendLine("{");
            source.AppendLine("    private readonly bool _shouldValidate;");
            source.AppendLine($"    public IRequestHandler<{c.RequestClassFullName}, {c.ResponseClassFullName}> RequestHandler {{ get; }}");
            source.AppendLine();
            source.AppendLine($"    public HttpContextRequestResolver_{c.RequestAsName}_{c.ResponseAsName}(IRequestHandler<{c.RequestClassFullName}, {c.ResponseClassFullName}> requestHandler)");
            source.AppendLine("    {");
            source.AppendLine("        RequestHandler = requestHandler;");
            source.AppendLine("        _shouldValidate = ((IHttpContextRequestResolver) this).Validate;");
            source.AppendLine("    }");
            source.AppendLine();
            source.AppendLine("    public Delegate EndpointDelegate =>");
            source.AppendLine($"        ([FromServices] IMediator mediator, {delegateBody.Description}, CancellationToken cancellationToken) =>");
            source.AppendLine($"            mediator.SendAsync({delegateBody.Declaration}, _shouldValidate, cancellationToken);");
            source.AppendLine("}");
            source.AppendLine();
        }
       
        return source.ToString();
    }

    private static (string Description, string Declaration) DelegateBody(EndpointClass endpointClass)
    {
        if (!endpointClass.Properties.Any(x => x.FromAttribute is not null)) 
            return ($"[FromBodyAttribute] {endpointClass.RequestClassFullName} obj", "obj");
        
        var declaration = new StringBuilder();
        var description = new StringBuilder($"new {endpointClass.RequestClassFullName}() {{ ");

        var properties = endpointClass.Properties.ToArray();
        
        for (var i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            var attributeText = property.FromAttribute is null 
                ? string.Empty 
                : $"[{property.FromAttribute}]";
            
            declaration.Append($"{attributeText} {property.Type} {property.Name.ToLower()}");
            description.Append($"{property.Name} = {property.Name.ToLower()}");

            if (i > properties.Length - 2) 
                continue;
            
            declaration.Append(", ");
            description.Append(", ");
        }

        description.Append(" }");
            
        return (declaration.ToString(), description.ToString());

    }
}
