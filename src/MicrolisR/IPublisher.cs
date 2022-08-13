namespace MicrolisR;

/// <summary>
/// Publish a notification or event to be handled by multiple subscribers.
/// </summary>
/// <seealso cref="INotification"/>
/// <seealso cref="ISubscriber{T}"/>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a notification to multiple subscribers.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}