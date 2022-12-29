namespace Segres.Abstractions;

/// <summary>
/// The delegate for intercepting a request with the <see cref="IRequestBehavior{TRequest,TResult}"/>. 
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate TResult RequestHandlerDelegate<TResult>(IRequest<TResult> request);

/// <inheritdoc />
public interface IRequestBehavior<in TRequest, TResult> : IAsyncRequestBehavior<TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    ValueTask<TResult> IAsyncRequestBehavior<TRequest, TResult>.HandleAsync(AsyncRequestHandlerDelegate<TResult> next, TRequest request, CancellationToken cancellationToken)
    {
        var requestDelegate = Next(next);
        var result = Handle(requestDelegate, request);
        return ValueTask.FromResult(result);
    }
    
    /// <summary>
    /// Hook into the pipeline of a request before calling the request handler or the next interceptor.
    /// </summary>
    /// <param name="next">The next step of the pipeline.</param>
    /// <param name="request">The request object.</param>
    /// <returns>The result of this interception.</returns>
    public TResult Handle(RequestHandlerDelegate<TResult> next, TRequest request);
    
    private static RequestHandlerDelegate<TResult> Next(AsyncRequestHandlerDelegate<TResult> next)
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