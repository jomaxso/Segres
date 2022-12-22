namespace Segres;

public interface IRequestBehavior<in TRequest, TResult> : IAsyncRequestBehavior<TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    ValueTask<TResult> IAsyncRequestBehavior<TRequest, TResult>.HandleAsync(AsyncRequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken)
    {
        var requestDelegate = Next(next);
        var result = Handle(requestDelegate, request);
        return ValueTask.FromResult(result);
    }

    public TResult Handle(RequestDelegate<TResult> next, TRequest request);
    
    private static RequestDelegate<TResult> Next(AsyncRequestDelegate<TResult> next)
    {
        return (r) =>
        {
            var resultTask = next(r, CancellationToken.None);

            if (resultTask.IsCompleted)
                return resultTask.Result;
            
            return resultTask.AsTask()
                .GetAwaiter()
                .GetResult();
        };
    }
}