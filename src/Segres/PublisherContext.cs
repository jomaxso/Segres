using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <summary>
/// Responsible for interacting with Notifications. Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="INotificationHandler{TNotification}"/>.
/// </summary>
public abstract class PublisherContext 
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SegresConvention _segresConvention;
    private readonly ConcurrentDictionary<Type, NotificationHandlerDefinition> _handlerCache = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    protected PublisherContext(IServiceProvider serviceProvider)
    {
        _segresConvention = serviceProvider.GetRequiredService<SegresConvention>();
        
        _serviceProvider = _segresConvention.ServiceLifetime is ServiceLifetime.Scoped 
            ? serviceProvider.CreateScope().ServiceProvider
            :serviceProvider;
    }

    /// <summary>
    /// Asynchronously handle the notification income.
    /// </summary>
    /// <param name="notification">The current notification.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    public abstract ValueTask OnPublishAsync(INotification notification, CancellationToken cancellationToken);

    /// <summary>
    /// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="INotificationHandler{TNotification}"/>.
    /// </summary>
    /// <param name="notification">The request object to consume.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask ConsumeAsync(INotification notification, CancellationToken cancellationToken)
    {
        var type = notification.GetType();

        var observerDefinition = _handlerCache.GetOrAdd(type, NotificationHandlerDefinition.Create);

        if (_serviceProvider.GetService(observerDefinition.HandlerType) is not object[] handlers)
            return ValueTask.CompletedTask;

        return observerDefinition.InvokeAsync(handlers, notification, _segresConvention.PublishInParallel, cancellationToken);
    }
}