namespace MicrolisR;

/// <summary>
/// Send a request to be handled by a single handler.
/// </summary>
/// <seealso cref="IRequest"/>
/// <seealso cref="IRequest{T}"/>
/// <seealso cref="IReceiver{T}"/>
/// <seealso cref="IReceiver{T, T}"/>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IReceiver{T, T}"/>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="IReceiver{T}"/>
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
}