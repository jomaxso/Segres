using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.Communication;

public sealed class Sender : ISender
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _requestHandlerCache = new();

    public Sender(IServiceProvider serviceProvider, SegresConfiguration? options = null)
    {
        _serviceProvider = options?.Lifetime is null or ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider
            : serviceProvider;
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

        if (_serviceProvider.GetService(requestDefinition.HandlerType) is not { } requestHandler)
            throw new Exception($"Missing handler registration for type {requestType.Name}");

        if (requestDefinition.HasPipeline is false)
            return requestDefinition.InvokeAsync(requestHandler, null, request, cancellationToken);

        var requestBehaviors = _serviceProvider.GetService(requestDefinition.BehaviorType) as object[];
        requestDefinition.CheckPipeline(requestBehaviors);
        return requestDefinition.InvokeAsync(requestHandler, requestBehaviors, request, cancellationToken);
    }
}