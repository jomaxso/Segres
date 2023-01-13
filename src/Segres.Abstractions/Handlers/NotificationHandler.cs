using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
    /// <inheritdoc/>
    ValueTask INotificationHandler<TNotification>.HandleAsync(TNotification notification, CancellationToken cancellationToken)
    {
        Handle(notification);
        return ValueTask.CompletedTask;
    }
    
    /// <summary>
    /// Asynchronously subscribe and handle a notification.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <seealso cref="INotification"/>
    protected abstract void Handle(TNotification notification);
}