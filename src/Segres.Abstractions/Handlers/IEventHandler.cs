namespace Segres;

/// <summary>
/// Defines a subscriber for a event.
/// </summary>
/// <seealso cref="IEvent"/>
public interface IEventHandler<in TEvent> 
    where TEvent : IEvent
{
    /// <summary>
    /// Asynchronously subscribe and handle a event.
    /// </summary>
    /// <param name="message">The event object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A ValueTask</returns>
    /// <seealso cref="IEvent"/>
    ValueTask HandleAsync(TEvent message, CancellationToken cancellationToken);
}