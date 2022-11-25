namespace Segres;

public delegate ValueTask<TResult> RequestDelegate<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);