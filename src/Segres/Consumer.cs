using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Segres.Contracts;

namespace Segres;

internal sealed class Consumer : IConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, NotificationHandlerDefinition> _handlerCache = new();

    private readonly bool _asParallel;

    public Consumer(IServiceProvider serviceProvider, bool asParallel = false)
    {
        _serviceProvider = serviceProvider;
        _asParallel = asParallel;
    }
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask ConsumeAsync(INotification notification, CancellationToken cancellationToken)
        => ConsumeAsync(notification, _asParallel, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask ConsumeAsync(INotification notification, bool asParallel, CancellationToken cancellationToken)
    {
        var type = notification.GetType();

        var observerDefinition = _handlerCache.GetOrAdd(type, NotificationHandlerDefinition.Create);

        if (_serviceProvider.GetService(observerDefinition.HandlerType) is not object[] handlers)
            return ValueTask.CompletedTask;

        return observerDefinition.InvokeAsync(handlers, notification, asParallel, cancellationToken);
    }
}