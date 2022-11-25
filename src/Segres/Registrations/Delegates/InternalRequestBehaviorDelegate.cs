namespace Segres;

internal delegate ValueTask<T> InternalRequestBehaviorDelegate<T>(object behaviors, object handler, IRequest<T> request, CancellationToken cancellationToken);