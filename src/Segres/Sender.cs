using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Segres.Abstractions;

namespace Segres;

internal sealed class Sender : ISender
{
    private readonly Func<Type, object?> _serviceResolver;
    private readonly ConcurrentDictionary<Type, object> _requestHandlerCache;

    public Sender(Func<Type, object?> serviceResolver, IEnumerable<KeyValuePair<Type, object>> requestHandlers)
    {
        _serviceResolver = serviceResolver;
        _requestHandlerCache = new ConcurrentDictionary<Type, object>(requestHandlers);
    }

    public void Send(IRequest request)
        => SendAsync(request, CancellationToken.None).Await();

    public async ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => await SendAsync((IRequest<None>) request, cancellationToken).ConfigureAwait(false);

    public TResponse Send<TResponse>(IRequest<TResponse> request)
        => SendAsync(request, CancellationToken.None).Await();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var requestDefinition = _requestHandlerCache.GetOrAdd<RequestHandlerDefinition<TResponse>>(requestType);

        if (_serviceResolver.GetService(requestDefinition.HandlerType) is not { } requestHandler)
            throw new Exception($"Missing handler registration as service for type {requestType.Name}");

        if (requestDefinition.HasPipeline is false)
            return requestDefinition.InvokeAsync(requestHandler, null, request, cancellationToken);

        var requestBehaviors = _serviceResolver.GetService(requestDefinition.BehaviorType) as object[];
        requestDefinition.CheckPipeline(requestBehaviors);
        return requestDefinition.InvokeAsync(requestHandler, requestBehaviors, request, cancellationToken);
    }

    public IAsyncEnumerable<TResponse> Send<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        => SendAsync(request, cancellationToken).Await();
}