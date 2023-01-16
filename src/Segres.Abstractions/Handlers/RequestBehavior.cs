namespace Segres;

/// <summary>
/// The delegate for intercepting a request with the <see cref="IRequestBehavior{TRequest,TResult}"/>. 
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate TResult SynchronisedRequestDelegate<TResult>(IRequest<TResult> request);

/// <inheritdoc />
public abstract class RequestBehavior<TRequest, TResult> : IRequestBehavior<TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    ValueTask<TResult> IRequestBehavior<TRequest, TResult>.HandleAsync(RequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken)
    {
        var requestDelegate = Next(next, cancellationToken);
        var result = Handle(requestDelegate, request);
        return ValueTask.FromResult(result);
    }
    
    /// <summary>
    /// Hook into the pipeline of a request before calling the request handler or the next interceptor.
    /// </summary>
    /// <param name="next">The next step of the pipeline.</param>
    /// <param name="request">The request object.</param>
    /// <returns>The result of this interception.</returns>
    protected abstract TResult Handle(SynchronisedRequestDelegate<TResult> next, TRequest request);
    
    private static SynchronisedRequestDelegate<TResult> Next(RequestDelegate<TResult> next, CancellationToken cancellationToken)
    {
        return (request) =>
        {
            var resultTask = next(request, cancellationToken);

            if (resultTask.IsCompleted)
                return resultTask.Result;
            
            return resultTask.AsTask()
                .GetAwaiter()
                .GetResult();
        };
    }
}