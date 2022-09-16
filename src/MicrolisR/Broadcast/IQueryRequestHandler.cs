namespace MicrolisR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IQueryRequest{T}"/>.</typeparam>
/// <typeparam name="TResponse">The response type of the request.</typeparam>
/// <seealso cref="IQueryRequest{T}"/>
public interface IQueryRequestHandler<in TRequest, TResponse> 
    where TRequest : IQueryRequest<TResponse>
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