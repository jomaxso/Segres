namespace Segres;

internal delegate ValueTask<T> InternalRequestResolverDelegate<T>(object handler, IRequest<T> request, CancellationToken cancellationToken);