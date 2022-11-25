namespace Segres;

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public interface INotificationHandler<in TMessage> 
    where TMessage : INotification
{
    /// <summary>
    /// Asynchronously subscribe and handle a message.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A Task</returns>
    /// <seealso cref="INotification"/>
    ValueTask HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}