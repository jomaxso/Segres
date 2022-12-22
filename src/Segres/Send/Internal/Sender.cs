using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Segres;

internal sealed class Sender : ISender
{
    private readonly ServiceResolver _serviceResolver;
    private readonly ConcurrentDictionary<Type, object> _requestHandlerCache = new();

    internal Sender(ServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => await SendAsync((IRequest<None>) request, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var requestDefinition = _requestHandlerCache.GetOrAdd<RequestHandlerDefinition<TResponse>>(requestType);

        if (_serviceResolver.GetService(requestDefinition.HandlerType) is not { } requestHandler)
            throw new Exception($"Missing handler registration for type {requestType.Name}");

        if (requestDefinition.HasPipeline is false)
            return requestDefinition.InvokeAsync(requestHandler, null, request, cancellationToken);

        var requestBehaviors = _serviceResolver.GetService(requestDefinition.BehaviorType) as object[];
        requestDefinition.CheckPipeline(requestBehaviors);
        return requestDefinition.InvokeAsync(requestHandler, requestBehaviors, request, cancellationToken);
    }

    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        var resultTask = SendAsync(request, CancellationToken.None);

        if (resultTask.IsCompleted)
            return resultTask.Result;

        return resultTask.AsTask()
            .GetAwaiter()
            .GetResult();
    }

    public void Send(IRequest request)
    {
        var resultTask = SendAsync(request, CancellationToken.None);

        if (resultTask.IsCompleted is false)
        {
            resultTask.AsTask()
                .GetAwaiter()
                .GetResult();
        }
    }
}