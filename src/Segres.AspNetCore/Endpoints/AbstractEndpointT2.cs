using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.AspNetCore;

/// <summary>
/// Represents an http endpoint with a request and a response. 
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public abstract class AbstractEndpoint<TRequest, TResponse> : IBaseEndpoint<TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
    /// <inheritdoc />
    public virtual void Configure(RouteHandlerBuilder routeBuilder)
    {
    }

    /// <inheritdoc />
    ValueTask<HttpResult<TResponse>> IRequestHandler<TRequest, HttpResult<TResponse>>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ResolveAsync(request, cancellationToken);

    ///<summary>
    /// Asynchronously receive and handle a http request.
    /// </summary>
    /// <param name="request">The request object implementing <see cref="IHttpRequest{TResponse}"/>.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    public abstract ValueTask<HttpResult<TResponse>> ResolveAsync(TRequest request, CancellationToken cancellationToken);

    protected HttpResult<T> Ok<T>(object? value = null)
        => new(Results.Ok(value));

    protected HttpResult<T> Ok<T>(T? value)
        => new(Results.Ok(value));

    protected HttpResult<T> Accepted<T>(string? uri = null, object? value = null)
        => new(Results.Accepted(uri, value));

    protected HttpResult<T> Accepted<T>(string? uri = null, T? value = default)
        => new(Results.Accepted(uri, value));

    protected HttpResult<T> Created<T>(Uri uri, object? value = null)
        => new(Results.Created(uri, value));

    protected HttpResult<T> CreatedAtRoute<T>(string? routeName = null, object? routeValues = null, object? value = null)
        => new(Results.CreatedAtRoute(routeName, routeValues, value));

    protected HttpResult<T> CreatedAtRoute<T>(string? routeName = null, object? routeValues = null, T? value = default)
        => new(Results.CreatedAtRoute(routeName, routeValues, value));

    protected HttpResult<T> Created<T>(string uri, object? value = null)
        => new(Results.Created(uri, value));

    protected HttpResult<T> NoContent<T>()
        => HttpResult<T>.NoContentResult;

    protected HttpResult<T> BadRequest<T>(object? error = null)
        => new(Results.BadRequest(error));

    protected HttpResult<T> BadRequest<T>(T? error)
        => new(Results.BadRequest(error));

    protected HttpResult<T> Unauthorized<T>() 
        => HttpResult<T>.UnauthorizedResult;

    protected HttpResult<T> Forbidden<T>(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
        => new(Results.Forbid(properties, authenticationSchemes));

    protected HttpResult<T> NotFound<T>(T value) 
        => new(Results.NotFound(value));

    protected HttpResult<T> NotFound<T>(object? value = null) 
        => new(Results.NotFound(value));

    protected HttpResult<T> Conflict<T>(object? error = null) 
        => new(Results.Conflict(error));

    protected HttpResult<T> Conflict<T>(T? error) 
        => new(Results.Conflict(error));

    protected HttpResult<T> UnprocessableEntity<T>(object? error = null) 
        => new(Results.UnprocessableEntity(error));

    protected HttpResult<T> UnprocessableEntity<T>(T? error) 
        => new(Results.UnprocessableEntity(error));
}