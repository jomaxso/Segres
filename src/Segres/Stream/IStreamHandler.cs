namespace Segres;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TStream"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IStreamHandler<in TStream, out TResponse>
    where TStream : IStreamRequest<TResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    IAsyncEnumerable<TResponse> HandleAsync(TStream stream, CancellationToken cancellationToken);
}