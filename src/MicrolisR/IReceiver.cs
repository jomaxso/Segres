namespace MicrolisR;

/// <summary>
/// Defines a receiver for a request. (just for internal usage) 
/// </summary>
/// <seealso cref="IReceiver{T}"/>
/// <seealso cref="IReceiver{T,T}"/>
public interface IReceiver
{
    internal Task ReceiveAsync<T>(T request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The response type of the request.</typeparam>
/// <seealso cref="IRequest{T}"/>
/// <seealso cref="IReceiver{T}"/>
public interface IReceiver<in TRequest, TResponse> : IReceiver
    where TRequest : IRequest<TResponse>
{
    Task IReceiver.ReceiveAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return ReceiveAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }

    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="IRequest{T}"/>
    Task<TResponse> ReceiveAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IReceiver{T,T}"/>

public interface IReceiver<in TRequest> : IReceiver<TRequest, None>
    where TRequest : IRequest
{
    Task IReceiver.ReceiveAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return ReceiveAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }

    async Task<None> IReceiver<TRequest, None>.ReceiveAsync(TRequest request, CancellationToken cancellationToken)
    {
        await ReceiveAsync(request, cancellationToken).ConfigureAwait(false);
        return new None();
    }


    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="IRequest"/>
    new Task ReceiveAsync(TRequest request, CancellationToken cancellationToken);
}