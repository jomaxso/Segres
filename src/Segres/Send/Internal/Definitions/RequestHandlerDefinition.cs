using System.Diagnostics;

namespace Segres;

internal class RequestHandlerDefinition<TResponse> : IHandlerDefinition<RequestHandlerDefinition<TResponse>>
{
    public bool? HasPipeline { get; private set; }
    public Type HandlerType { get; }
    public Type BehaviorType { get; }

    public PipelineDelegate<TResponse> InvokeAsync { get; }

    public static RequestHandlerDefinition<TResponse> Create(Type requestType) => new(requestType);

    private RequestHandlerDefinition(Type requestType)
    {
        var responseType = typeof(TResponse);
        var self = this.GetType();

        this.HandlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, responseType);

        this.BehaviorType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(IRequestBehavior<,>)
                .MakeGenericType(requestType, responseType));

        this.InvokeAsync = (PipelineDelegate<TResponse>) self
            .CreateInternalDelegate(nameof(CreateInterceptorRequestDelegate), requestType);
    }

    private static PipelineDelegate<TResponse> CreateInterceptorRequestDelegate<TRequest>()
        where TRequest : IRequest<TResponse>
    {
        return (requestHandler, requestBehaviors, request, cancellationToken) =>
        {
            if (requestHandler is not IRequestHandler<TRequest, TResponse> handler)
                throw new UnreachableException();

            if (requestBehaviors is not IRequestBehavior<TRequest, TResponse>[] behaviors)
                return handler.HandleAsync((TRequest) request, cancellationToken);

            var requestDelegate = Create(behaviors, behaviors.Length - 1, (r, c) => handler.HandleAsync((TRequest) r, c));
            return requestDelegate.Invoke(request, cancellationToken);
        };
    }

    private static RequestDelegate<TResult> Create<TRequest, TResult>(IRequestBehavior<TRequest, TResult>[] handlers, int index, RequestDelegate<TResult> finalDelegate)
        where TRequest : IRequest<TResult>
        => index < 0
            ? finalDelegate
            : (r, c) => handlers[index].HandleAsync(Create(handlers, index - 1, finalDelegate), (TRequest) r, c);

    public void CheckPipeline(object[]? requestBehaviors) 
        => HasPipeline ??= requestBehaviors is not null && requestBehaviors.Length > 0;
}