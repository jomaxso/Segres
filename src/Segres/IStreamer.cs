using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// 
/// </summary>
public interface IStreamer
{
    /// <summary>
    /// Asynchronously receive a streamRequest from a single handler.
    /// </summary>
    /// <param name="streamRequest"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns>A streamRequest as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamHandler{TStream,TResult}"/>
    IAsyncEnumerable<TResponse> CreateStreamAsync<TResponse>(IStreamRequest<TResponse> streamRequest, CancellationToken cancellationToken = default);
}