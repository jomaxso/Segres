using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public interface INotificationHandler<in TNotification> : IAsyncNotificationHandler<TNotification>
    where TNotification : INotification
{
    ValueTask IAsyncNotificationHandler<TNotification>.HandleAsync(TNotification notification, CancellationToken cancellationToken)
    {
        Handle(notification);
        return ValueTask.CompletedTask;
    }
    
    /// <summary>
    /// Asynchronously subscribe and handle a notification.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <seealso cref="INotification"/>
    void Handle(TNotification notification);
}