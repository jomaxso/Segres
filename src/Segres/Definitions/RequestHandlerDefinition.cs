using System.Diagnostics;
using Segres.Abstractions;

namespace Segres;

internal delegate ValueTask<T> PipelineDelegate<T>(object handler, object[]? behaviors, IRequest<T> request, CancellationToken cancellationToken);

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
        
        this.HandlerType = typeof(IAsyncRequestHandler<,>)
            .MakeGenericType(requestType, responseType);

        this.BehaviorType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(IAsyncRequestBehavior<,>)
                .MakeGenericType(requestType, responseType));

        this.InvokeAsync = (PipelineDelegate<TResponse>) self
            .CreateInternalDelegate(nameof(CreateInterceptorRequestDelegate), requestType);
    }

    private static PipelineDelegate<TResponse> CreateInterceptorRequestDelegate<TRequest>()
        where TRequest : IRequest<TResponse>
    {
        return (requestHandler, requestBehaviors, request, cancellationToken) =>
        {
            if (requestHandler is not IAsyncRequestHandler<TRequest, TResponse> handler)
                throw new UnreachableException();

            if (requestBehaviors is not IAsyncRequestBehavior<TRequest, TResponse>[] behaviors || behaviors.Length < 1)
                return handler.HandleAsync((TRequest) request, cancellationToken);

            return Create(behaviors, behaviors.Length - 1, handler.ExecuteRequestHandler)
                .Invoke(request, cancellationToken);
        };
    }

    private static AsyncRequestHandlerDelegate<TResult> Create<TRequest, TResult>(IAsyncRequestBehavior<TRequest, TResult>[] handlers, int index, AsyncRequestHandlerDelegate<TResult> finalHandlerDelegate)
        where TRequest : IRequest<TResult>
        => index < 0
            ? finalHandlerDelegate
            : (r, c) => handlers[index].HandleAsync(Create(handlers, index - 1, finalHandlerDelegate), (TRequest) r, c);

    public void CheckPipeline(object[]? requestBehaviors)
        => HasPipeline ??= requestBehaviors is not null && requestBehaviors.Length > 0;
}