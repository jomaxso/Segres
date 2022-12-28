using System.Diagnostics;
using Segres.Behaviors;
using Segres.Contracts;
using Segres.Delegates;
using Segres.Extensions;
using Segres.Handlers;

namespace Segres.Definitions;

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

    private static AsyncRequestDelegate<TResult> Create<TRequest, TResult>(IAsyncRequestBehavior<TRequest, TResult>[] handlers, int index, AsyncRequestDelegate<TResult> finalDelegate)
        where TRequest : IRequest<TResult>
        => index < 0
            ? finalDelegate
            : (r, c) => handlers[index].HandleAsync(Create(handlers, index - 1, finalDelegate), (TRequest) r, c);

    public void CheckPipeline(object[]? requestBehaviors)
        => HasPipeline ??= requestBehaviors is not null && requestBehaviors.Length > 0;
}