using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Definitions;
using Segres.Delegates;
using Segres.Extensions;

namespace Segres;

public sealed class Subscriber : ISubscriber
{
    private readonly ServiceResolver _serviceResolver;
    private readonly ConcurrentDictionary<Type, NotificationHandlerDefinition> _handlerCache = new();

    private readonly bool _asParallel;

    public Subscriber(ServiceResolver serviceResolver, bool asParallel = false)
    {
        _serviceResolver = serviceResolver;
        _asParallel = asParallel;
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

        if (_serviceResolver.GetService(observerDefinition.HandlerType) is not object[] handlers)
            return ValueTask.CompletedTask;

        return observerDefinition.InvokeAsync(handlers, notification, asParallel, cancellationToken);
    }
}