namespace MicrolisR;

/// <summary>
/// Publish a notification or event to be handled by multiple subscribers.
/// </summary>
/// <seealso cref="INotification"/>
/// <seealso cref="INotificationHandler{TNotification}"/>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a notification to multiple subscribers.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
        where TNotification : INotification;
    
    /// <summary>
    /// Send a notification to multiple subscribers.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <returns>A task that represents the publish operation.</returns>
    void Publish<TNotification>(TNotification notification) 
        where TNotification : INotification;
}