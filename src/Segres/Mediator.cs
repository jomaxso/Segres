using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

internal sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _requestHandlerCache;

    public Mediator(IServiceProvider serviceProvider, IEnumerable<KeyValuePair<Type, object>> requestHandlers)
    {
        _serviceProvider = serviceProvider;
        _requestHandlerCache = new ConcurrentDictionary<Type, object>(requestHandlers);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var requestDefinition = _requestHandlerCache.GetOrAdd<RequestHandlerDefinition<TResponse>>(requestType);

        if (_serviceProvider.GetService(requestDefinition.HandlerType) is not { } requestHandler)
            throw new Exception($"Missing handler registration as service for type {requestType.Name}");

        if (requestDefinition.HasPipeline is false)
            return requestDefinition.InvokeAsync(requestHandler, null, request, cancellationToken);
        
        var requestBehaviors = _serviceProvider.GetService(requestDefinition.BehaviorType) as object[];
        requestDefinition.CheckPipeline(requestBehaviors);
        return requestDefinition.InvokeAsync(requestHandler, requestBehaviors, request, cancellationToken);
    }

    /// <inheritdoc />
    public void Send(IRequest request)
    {
        var valueTask = SendAsync(request, CancellationToken.None);
        Await(valueTask);
    }

    /// <inheritdoc />
    public async ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => await SendAsync((IRequest<None>)request, cancellationToken);

    /// <inheritdoc />
    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        var valueTask = SendAsync(request, CancellationToken.None);
        return Await(valueTask);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TResponse> Send<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var valueTask = SendAsync(request, cancellationToken);
        return Await(valueTask);
    }

    /// <inheritdoc />
    public ValueTask<IAsyncEnumerable<TResult>> SendAsync<TResult>(IStreamRequest<TResult> request, CancellationToken cancellationToken = default)
        => SendAsync((IRequest<IAsyncEnumerable<TResult>>)request, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default) 
        where TEvent : IEvent
        => _serviceProvider.GetRequiredService<PublisherContext>().OnPublishAsync(message, cancellationToken);

    public void Publish<TEvent>(TEvent message) where TEvent : IEvent
    {
        var valueTask = PublishAsync(message, CancellationToken.None);
        Await(valueTask);
    }
    
    private static void Await(ValueTask valueTask)
    {
        if (valueTask.IsCompleted)
            return;

        valueTask.AsTask()
            .GetAwaiter()
            .GetResult();
    }
    
    private static TResult Await<TResult>(ValueTask<TResult> valueTask)
    {
        if (valueTask.IsCompleted)
            return valueTask.Result;

        return valueTask.AsTask()
            .GetAwaiter()
            .GetResult();
    }
}