﻿using Microsoft.AspNetCore.Http;

namespace Segres.AspNetCore;

/// <summary>
/// A interceptor for a request executed before calling the <see cref="AbstractEndpoint{TRequest,TResponse}"/>.
/// </summary>
/// <typeparam name="TRequest">Th request type. Has to implement <see cref="IHttpRequest{TResult}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the result specified from the <see cref="IRequest{TResult}"/> where the result result type implements <see cref="IHttpResult{TSelf,T}"/>.</typeparam>
public abstract class AbstractEndpointBehavior<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IHttpResult<TResponse, IResult>
{
    /// <inheritdoc />
    public abstract ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken);
}