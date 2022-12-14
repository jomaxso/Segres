using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

internal sealed class Subscriber : ISubscriber
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, NotificationHandlerDefinition> _handlerCache = new();

    private readonly bool _asParallel;

    public Subscriber(IServiceProvider serviceProvider, SegresConfiguration? options = null)
    {
        _serviceProvider = options?.Lifetime is null or ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider
            : serviceProvider;

        _asParallel = options?.PublisherStrategy ?? false;
    }
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SubscribeAsync(INotification notification, CancellationToken cancellationToken)
        => SubscribeAsync(notification, _asParallel, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SubscribeAsync(INotification notification, bool asParallel, CancellationToken cancellationToken)
    {
        var type = notification.GetType();

        var observerDefinition = _handlerCache.GetOrAdd(type, NotificationHandlerDefinition.Create);

        if (_serviceProvider.GetService(observerDefinition.HandlerType) is not object[] handlers)
            return ValueTask.CompletedTask;

        return observerDefinition.InvokeAsync(handlers, notification, asParallel, cancellationToken);
    }
}