using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TStream"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IStreamHandler<in TStream, out TResult>
    where TStream : IStream<TResult>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    IAsyncEnumerable<TResult> HandleAsync(TStream stream, CancellationToken cancellationToken = default);
}