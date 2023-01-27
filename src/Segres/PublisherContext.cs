using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <summary>
/// Responsible for interacting with events. Consumes a <see cref="IEvent"/> and sends it to the registered <see cref="IEventHandler{TEvent}"/> or <see cref="IEventHandler{TEvent}"/>.
/// </summary>
public abstract class PublisherContext 
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, NotificationHandlerDefinition> _handlerCache = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    protected PublisherContext(IServiceProvider serviceProvider)
    {
        var segresConvention = serviceProvider.GetRequiredService<SegresConvention>();
        
        _serviceProvider = segresConvention.ServiceLifetime is ServiceLifetime.Scoped 
            ? serviceProvider.CreateScope().ServiceProvider
            :serviceProvider;
    }

    /// <summary>
    /// Asynchronously handle the event income.
    /// </summary>
    /// <param name="event">The current event.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    public abstract ValueTask OnPublishAsync(IEvent @event, CancellationToken cancellationToken);

    /// <summary>
    /// Consumes a <see cref="IEvent"/> and sends it to the registered <see cref="IEventHandler{TEvent}"/> or <see cref="IEventHandler{TEvent}"/>.
    /// </summary>
    /// <param name="event">The request object to consume.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask ConsumeAsync(IEvent @event, CancellationToken cancellationToken)
    {
        var type = @event.GetType();

        var observerDefinition = _handlerCache.GetOrAdd(type, NotificationHandlerDefinition.Create);

        if (_serviceProvider.GetService(observerDefinition.HandlerType) is not object[] handlers)
            return ValueTask.CompletedTask;

        return observerDefinition.InvokeAsync(handlers, @event, cancellationToken);
    }
}