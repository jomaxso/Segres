using Segres.Contracts;

namespace Segres;

/// <summary>
/// Responsible for interacting with Notifications.
/// </summary>
public interface IPublisherContext
{
    /// <summary>
    /// Asynchronously handle the notification flow.
    /// </summary>
    /// <param name="notification">The current notification.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    /// <remarks>
    /// To consume a message use the <see cref="IConsumer"/>.
    /// </remarks>
    ValueTask RaiseNotificationAsync(INotification notification, CancellationToken cancellationToken);
}