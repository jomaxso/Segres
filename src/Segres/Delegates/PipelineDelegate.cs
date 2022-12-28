using Segres.Contracts;

namespace Segres.Delegates;

internal delegate ValueTask<T> PipelineDelegate<T>(object handler, object[]? behaviors, IRequest<T> request, CancellationToken cancellationToken);