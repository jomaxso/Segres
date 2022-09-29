using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TQuery">The request type witch implements <see cref="IQuery{T}"/>.</typeparam>
/// <typeparam name="TResult">The response type of the request.</typeparam>
/// <seealso cref="IQuery{T}"/>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQuery{T}"/>
    Task<TResult> HandleAsync(TQuery request, CancellationToken cancellationToken = default);
}