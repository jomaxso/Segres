using Segres.Contracts;

namespace Segres.Behaviors;

public delegate ValueTask<TResult> AsyncRequestDelegate<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);