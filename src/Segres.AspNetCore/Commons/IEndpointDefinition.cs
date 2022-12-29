using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace Segres.AspNetCore;

public interface IEndpointDefinition
{
    IEndpointDefinition WithGroup(string group);
    RouteHandlerBuilder MapPost([StringSyntax("Route")] string route = "/");
    RouteHandlerBuilder MapGet([StringSyntax("Route")] string route = "/");
    RouteHandlerBuilder MapPut([StringSyntax("Route")] string route = "/");
    RouteHandlerBuilder MapDelete([StringSyntax("Route")] string route = "/");
    RouteHandlerBuilder MapFromAttribute();
}