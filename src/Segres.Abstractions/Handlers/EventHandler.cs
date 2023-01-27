namespace Segres;

/// <summary>
/// Defines a subscriber for a event.
/// </summary>
/// <seealso cref="IEvent"/>
public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : IEvent
{
    /// <inheritdoc/>
    ValueTask IEventHandler<TEvent>.HandleAsync(TEvent message, CancellationToken cancellationToken)
    {
        Handle(message);
        return ValueTask.CompletedTask;
    }
    
    /// <summary>
    /// Asynchronously subscribe and handle a event.
    /// </summary>
    /// <param name="message">The event object</param>
    /// <seealso cref="IEvent"/>
    protected abstract void Handle(TEvent message);
}