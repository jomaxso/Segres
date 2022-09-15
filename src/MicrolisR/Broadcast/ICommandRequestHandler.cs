namespace MicrolisR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="ICommandRequest"/>.</typeparam>
/// <seealso cref="ICommandRequest"/>
/// <seealso cref="ICommandRequestHandler{TRequest}"/>
public interface ICommandRequestHandler<in TRequest> : IInternalRequestHandler
    where TRequest : ICommandRequest
{
    Task IInternalRequestHandler.HandleAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return HandleAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }
    
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
public interface ICommandRequestHandler<in TRequest, TResponse> : IInternalRequestHandler
    where TRequest : ICommandRequest<TResponse>
{
    Task IInternalRequestHandler.HandleAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return HandleAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }

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