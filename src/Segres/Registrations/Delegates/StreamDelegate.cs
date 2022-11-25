namespace Segres;

internal delegate IAsyncEnumerable<T> StreamDelegate<T>(object handler, IStreamRequest<T> streamRequest, CancellationToken cancellationToken);