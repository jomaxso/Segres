using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace Segres.AspNet;

public abstract class EndpointDefinition
{
    protected string Route { get; private set; } = "/";
    protected string Group { get; private set; } = string.Empty;

    public EndpointDefinition WithRoute([StringSyntax("Route")]string route)
    {
        this.Route = route;
        return this;
    }

    public EndpointDefinition WithGroup(string group)
    {
        this.Group = group;
        return this;
    }

    public abstract RouteHandlerBuilder MapPost();
    public abstract RouteHandlerBuilder MapGet();
    public abstract RouteHandlerBuilder MapPut();
    public abstract RouteHandlerBuilder MapDelete();
}