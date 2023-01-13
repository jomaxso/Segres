using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public interface INotificationHandler<in TNotification> 
    where TNotification : INotification
{
    /// <summary>
    /// Asynchronously subscribe and handle a notification.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A Task</returns>
    /// <seealso cref="INotification"/>
    ValueTask HandleAsync(TNotification notification, CancellationToken cancellationToken);
}