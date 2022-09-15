namespace MicrolisR;

/// <summary>
/// Send a request to be handled by a single handler.
/// </summary>
/// <seealso cref="ICommandRequest"/>
/// <seealso cref="ICommandRequestHandler{TRequest}"/>
/// <seealso cref="IQueryRequest{T}"/>
/// <seealso cref="IQueryRequestHandler{TRequest,TResponse}"/>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="ICommandRequestHandler{TRequest}"/>
    Task SendAsync(ICommandRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandRequestHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryRequestHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default);
}