using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="IEvent"/>
public interface IEventHandler<in TEvent> 
    where TEvent : IEvent
{
    /// <summary>
    /// Asynchronously subscribe and handle a message.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A Task</returns>
    /// <seealso cref="IEvent"/>
    Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}