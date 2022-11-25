using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Segres.Tmp.Http;

namespace Segres;

/// <inheritdoc />
internal sealed class Sender : ISender
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _requestHandlerCache = new();
    private readonly ConcurrentDictionary<Type, object> _httpRequestHandlerCache = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public Sender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => await SendAsync((IRequest<None>) request, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var requestDefinition = _requestHandlerCache.GetOrAdd<RequestDefinition<TResult>>(requestType);
        
        var requestHandler = _serviceProvider.GetService(requestDefinition.HandlerType);
        
        if (requestHandler is null)
            throw new Exception($"Missing handler registration for type {requestType.Name}");

        if (requestDefinition.HasPipeline)
        {
            var requestBehaviors = _serviceProvider.GetService(requestDefinition.BehaviorType);
            requestDefinition.HasPipeline = requestBehaviors is object[] {Length: > 1};

            if (requestBehaviors is not null)
                return requestDefinition.InvokePipeline(requestBehaviors, requestHandler, request, cancellationToken);
        }

        return requestDefinition.InvokeHandler(requestHandler, request, cancellationToken);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResult> SendAsync<TResult>(IHttpRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInfo = _httpRequestHandlerCache.GetOrAdd<HttpRequestRunner<TResult>>(requestType);

        var handler = _serviceProvider.GetService(handlerInfo.HandlerType);

        if (handler is null)
            throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return handlerInfo.InvokeHandlerAsync(handler, request, cancellationToken);
    }
}