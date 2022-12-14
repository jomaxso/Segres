using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Segres.AspNet;

internal class RequestEndpointDefinition<TRequest, TResponse> : EndpointDefinition
    where TRequest : notnull
{
    private readonly IEndpointRouteBuilder _routeBuilder;

    public RequestEndpointDefinition(IEndpointRouteBuilder routeBuilder)
    {
        _routeBuilder = routeBuilder;
    }

    public override RouteHandlerBuilder MapPost()
    {
        return _routeBuilder.MapGroup(Group)
            .MapPost(this.Route,
                (ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken) => sender.SendAsync(new EndpointRequest<TRequest>(request), cancellationToken));
    }

    public override RouteHandlerBuilder MapGet()
    {
        return _routeBuilder.MapGroup(Group)
            .MapGet(this.Route,
                (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => sender.SendAsync(new EndpointRequest<TRequest>(request), cancellationToken));
    }

    public override RouteHandlerBuilder MapPut()
    {
        return _routeBuilder.MapGroup(Group)
            .MapPut(this.Route,
                (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => sender.SendAsync(new EndpointRequest<TRequest>(request), cancellationToken));
    }

    public override RouteHandlerBuilder MapDelete()
    {
        return _routeBuilder.MapGroup(Group)
            .MapDelete(this.Route,
                (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) => sender.SendAsync(new EndpointRequest<TRequest>(request), cancellationToken));
    }
}