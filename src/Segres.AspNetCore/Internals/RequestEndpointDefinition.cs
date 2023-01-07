using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Segres.Abstractions;

namespace Segres.AspNetCore;

internal class RequestEndpointDefinition<TRequest, TResponse> : EndpointDefinition
    where TRequest : IRequest<TResponse>
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
            .MapPost(route, (ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken) =>
                sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapGet([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapGet(route,
                (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) =>
                    sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapPut([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapPut(route, (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) =>
                sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken));
    }

    protected internal override RouteHandlerBuilder InternalMapDelete([StringSyntax("Route")] string route, string group = "")
    {
        return _routeBuilder.MapGroup(group)
            .WithTags(group)
            .MapDelete(route, (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) =>
                sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken));
    }
}