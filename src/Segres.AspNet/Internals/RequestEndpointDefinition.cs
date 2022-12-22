using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Segres.AspNet;

internal class RequestEndpointDefinition<TRequest, TResponse> : EndpointDefinition
    where TRequest : IHttpRequest<TResponse>
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public RequestEndpointDefinition(IEndpointRouteBuilder routeBuilder) : base(typeof(TRequest))
    {
        _routeBuilder = routeBuilder;
    }

    protected internal override RouteHandlerBuilder InternalMapPost([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapPost(route,
                async (ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken) =>
                {
                    var result =  await sender.SendAsync(request, cancellationToken);
                    return result.ToResult();
                });
    }

    protected internal override RouteHandlerBuilder InternalMapGet([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapGet(route,
                async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapPut([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapPut(route, async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapDelete([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapDelete(route, async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }
}



internal class RequestEndpointDefinition<TRequest> : EndpointDefinition
    where TRequest : IHttpRequest
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public RequestEndpointDefinition(IEndpointRouteBuilder routeBuilder) : base(typeof(TRequest))
    {
        _routeBuilder = routeBuilder;
    }

    protected internal override RouteHandlerBuilder InternalMapPost([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapPost(route,
                async (ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapGet([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapGet(route,
                async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapPut([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapPut(route, async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapDelete([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapDelete(route, async (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => await sender.SendAsync(request, cancellationToken));
    }
}