namespace Segres;

internal delegate ValueTask<T> PipelineDelegate<T>(object handler, object[]? behaviors, IRequest<T> request, CancellationToken cancellationToken);