namespace MicrolisR;

/// <summary>
/// Send a query to be handled by a single handler.
/// </summary>
/// <seealso cref="IQuery{T}"/>
/// <seealso cref="IQueryHandler{TRequest,TResponse}"/>
public interface IQuerySender
{
    /// <summary>
    /// Send a query to a single Receiver.
    /// </summary>
    /// <param name="request">The query object</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TRequest,TResponse}"/>
    TResponse Send<TResponse>(IQuery<TResponse> request);
    
    /// <summary>
    /// Asynchronously send a query to a single Receiver.
    /// </summary>
    /// <param name="request">The query object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);
}