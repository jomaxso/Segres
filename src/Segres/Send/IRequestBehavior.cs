namespace Segres;

public interface IRequestBehavior<in TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    public ValueTask<TResult> HandleAsync(RequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken);
}