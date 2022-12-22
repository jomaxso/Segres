namespace Segres;

public delegate ValueTask<TResult> AsyncRequestDelegate<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);

public delegate TResult RequestDelegate<TResult>(IRequest<TResult> request);
