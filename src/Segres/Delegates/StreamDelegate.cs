using Segres.Contracts;

namespace Segres.Delegates;

internal delegate IAsyncEnumerable<T> StreamDelegate<T>(object handler, IStreamRequest<T> streamRequest, CancellationToken cancellationToken);