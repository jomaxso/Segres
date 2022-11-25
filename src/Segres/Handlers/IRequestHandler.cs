namespace Segres;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IRequestHandler{TCommand}"/>
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, None>
    where TRequest : IRequest
{
    async ValueTask<None> IRequestHandler<TRequest, None>.HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return None.Empty;
    }

    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="IRequest"/>
   new ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest{TCommand}"/>.</typeparam>
/// <typeparam name="TResult"></typeparam>
/// <seealso cref="IRequest{TResult}"/>
/// <seealso cref="IRequestHandler{TRequest, TResult}"/>
public interface IRequestHandler<in TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest{T}"/>
    ValueTask<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}