namespace MicrolisR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="ICommandRequest"/>.</typeparam>
/// <seealso cref="ICommandRequest"/>
/// <seealso cref="ICommandRequestHandler{TRequest}"/>
public interface ICommandRequestHandler<in TRequest> 
    where TRequest : ICommandRequest
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="ICommandRequest"/>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="ICommandRequest"/>.</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <seealso cref="ICommandRequest{TResponse}"/>
/// <seealso cref="ICommandRequestHandler{TRequest}"/>
public interface ICommandRequestHandler<in TRequest, TResponse> 
    where TRequest : ICommandRequest<TResponse>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="IQueryRequest{T}"/>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}