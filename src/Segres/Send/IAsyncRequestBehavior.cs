namespace Segres;

public interface IAsyncRequestBehavior<in TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    public ValueTask<TResult> HandleAsync(AsyncRequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken);
}