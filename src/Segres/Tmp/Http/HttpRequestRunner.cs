using System.Diagnostics;

namespace Segres.Tmp.Http;

internal class HttpRequestRunner<TResponse> : 
    IRequestConstructor<HttpRequestRunner<TResponse>>
{
    private const string MethodName = nameof(CreateRequestDelegate);
    
    private readonly HttpRequestDelegate<TResponse> _handlerMethod;
    
    public Type HandlerType { get; }
    
    public ValueTask<TResponse> InvokeHandlerAsync(object handler, IHttpRequest<TResponse> request, CancellationToken cancellationToken, object? interceptors = null)
    {
        return _handlerMethod.Invoke(handler, request, cancellationToken);
    }
    
    public static HttpRequestRunner<TResponse> Create(Type requestType) => new(requestType);

    
    private HttpRequestRunner(Type requestType)
    {
        this.HandlerType = typeof(HttpRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        this._handlerMethod = (HttpRequestDelegate<TResponse>)this.GetType()
            .CreateInternalDelegate(MethodName, requestType);
    }

    private static HttpRequestDelegate<TResponse?> CreateRequestDelegate<TRequest>() 
        where TRequest : IHttpRequest<TResponse?> 
        => (handler, request, cancellationToken) 
            => ((HttpRequestHandler<TRequest, TResponse?>) handler).HandleAsync(new HttpRequest<TRequest, TResponse?>((TRequest)request), cancellationToken);
}