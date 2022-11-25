using System.Diagnostics;
using Segres.Tmp;

namespace Segres;

internal interface IRequestConstructor<out TSelf>
{
    public static abstract TSelf Create(Type requestType);
}

internal class RequestDefinition<TResponse> : IRequestConstructor<RequestDefinition<TResponse>>
{
    private bool? _hasPipeline;
    
    private const string HandlerMethodName = nameof(CreateRequestDelegate);
    private const string PipelineMethodName = nameof(CreateInterceptorRequestDelegate);

    private readonly InternalRequestResolverDelegate<TResponse> _handlerMethod;
    private readonly InternalRequestBehaviorDelegate<TResponse> _pipelineMethod;
    
    public Type HandlerType { get; }
    public Type BehaviorType { get; }
    
    public bool HasPipeline
    {
        get =>  _hasPipeline is true or null;
        set => _hasPipeline ??= value;
    }
    
    public static RequestDefinition<TResponse> Create(Type requestType) => new(requestType);
    
    public ValueTask<TResponse> InvokePipeline(object requestBehavior, object requestHandler, IRequest<TResponse> request, CancellationToken cancellationToken) 
        => _pipelineMethod.Invoke(requestBehavior, requestHandler, request, cancellationToken);

    public ValueTask<TResponse> InvokeHandler(object requestHandler, IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        // TODO Requestfilter müssen hier erst ausgeführt werden
        
        return _handlerMethod.Invoke(requestHandler, request, cancellationToken);
    }

    private RequestDefinition(Type requestType)
    {
        var responseType = typeof(TResponse);
        var self = this.GetType();
        
        this.HandlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, responseType);

        this.BehaviorType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(IRequestBehavior<,>)
                .MakeGenericType(requestType, responseType));

        this._handlerMethod = (InternalRequestResolverDelegate<TResponse>) self
            .CreateInternalDelegate(HandlerMethodName, requestType);

        this._pipelineMethod = (InternalRequestBehaviorDelegate<TResponse>) self
            .CreateInternalDelegate(PipelineMethodName, new[] {requestType}, this);
    }
    
    private static InternalRequestBehaviorDelegate<TResponse> CreateInterceptorRequestDelegate<TRequest>(RequestDefinition<TResponse> requestDefinition)
        where TRequest : IRequest<TResponse>
    {
        return (interceptors, handler, request, cancellationToken) =>
        {
            var requestDelegate = interceptors switch
            {
                IRequestBehavior<TRequest, TResponse> singleHandler => 
                    (r, c) => singleHandler.HandleAsync((rr, cc) => requestDefinition.InvokeHandler(handler, rr, cc), (TRequest) r, c),
                
                IRequestBehavior<TRequest, TResponse>[] listOfHandler => 
                    Create(listOfHandler, listOfHandler.Length - 1, (r, c) => requestDefinition.InvokeHandler(handler, r, c)),
                
                _ => throw new UnreachableException()
            };

            return requestDelegate.Invoke(request, cancellationToken);
        };
    }
    
    private static InternalRequestResolverDelegate<TResponse> CreateRequestDelegate<TRequest>()
        where TRequest : IRequest<TResponse> => (handler, query, cancellationToken)
            => ((IRequestHandler<TRequest, TResponse>) handler).HandleAsync((TRequest) query, cancellationToken);
    
    private static RequestDelegate<TResult> Create<TRequest, TResult>(IRequestBehavior<TRequest, TResult>[] handlers, int index, RequestDelegate<TResult> finalDelegate)
        where TRequest : IRequest<TResult> 
        => index < 0 ? finalDelegate : (r, c) => handlers[index].HandleAsync(Create(handlers, index - 1, finalDelegate), (TRequest)r, c);
}