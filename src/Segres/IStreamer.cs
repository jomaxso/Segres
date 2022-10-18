using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// 
/// </summary>
public interface IStreamer
{
    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamHandler{TStream,TResult}"/>
    IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStream<TResult> stream, CancellationToken cancellationToken = default);
}