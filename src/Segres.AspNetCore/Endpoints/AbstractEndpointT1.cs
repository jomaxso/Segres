using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Segres.Handlers;

namespace Segres.AspNetCore;

/// <summary>
/// Represents an http endpoint with a request and no response. 
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public abstract class AbstractEndpoint<TRequest> : IBaseEndpoint<TRequest, None>
    where TRequest : IHttpRequest
{
    /// <inheritdoc />
    public virtual void Configure(RouteHandlerBuilder routeBuilder)
    {
    }

    /// <inheritdoc />
    async ValueTask<HttpResult<None>> IRequestHandler<TRequest, HttpResult<None>>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => await ResolveAsync(request, cancellationToken);
    
    ///<summary>
    /// Asynchronously receive and handle a http request.
    /// </summary>
    /// <param name="request">The request object implementing <see cref="IHttpRequest"/>.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    public abstract ValueTask<HttpResult> ResolveAsync(TRequest request, CancellationToken cancellationToken);

    protected HttpResult Ok() 
        => HttpResult.OkResult;
    
    protected HttpResult NoContent() 
        => HttpResult.NoContentResult;
    
    protected HttpResult Unauthorized()
        => HttpResult.UnauthorizedResult;

    protected HttpResult Accepted(string? uri = null, object? value = null)
        => new(Results.Accepted(uri, value));
    
    protected HttpResult Created(Uri uri, object? value = null) 
        => new(Results.Created(uri, value));

    protected HttpResult Created(string uri, object? value = null) 
        => new(Results.Created(uri, value));

    protected HttpResult CreatedAtRoute(string? routeName = null, object? routeValues = null, object? value = null)
        => new(Results.CreatedAtRoute(routeName, routeValues, value));

    protected HttpResult BadRequest(object? error = null)
        => new(Results.BadRequest(error));

    protected HttpResult Forbidden(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
        => new(Results.Forbid(properties, authenticationSchemes));

    protected HttpResult NotFound(object? error = null)
        => new(Results.NotFound(error));

    protected HttpResult UnprocessableEntity(object? error = null)
        => new(Results.UnprocessableEntity(error));

    protected HttpResult Conflict(object? error = null) 
        => new(Results.Conflict(error));

    protected HttpResult SignIn(ClaimsPrincipal principal, AuthenticationProperties? properties = null, string? authenticationScheme = null) 
        => new(Results.SignIn(principal, properties, authenticationScheme));

    protected HttpResult SignOut(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
        => new(Results.SignOut(properties, authenticationSchemes));
}