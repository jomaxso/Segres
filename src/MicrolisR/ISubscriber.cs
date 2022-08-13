namespace MicrolisR;

/// <summary>
/// Defines a subscriber for a notification. (just for internal usage) 
/// </summary>
/// <seealso cref="ISubscriber{TNotification}"/>
public interface ISubscriber
{
    internal Task SubscribeAsync<TNotification>(TNotification notification, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public interface ISubscriber<in TNotification> : ISubscriber
    where TNotification : INotification
{
    Task ISubscriber.SubscribeAsync<T>(T notification, CancellationToken cancellationToken)
    {
        if (notification is TNotification subscribable)
            return SubscribeAsync(subscribable, cancellationToken);
        
        throw new ArgumentException($"The message is not of type {typeof(TNotification)}");
    }
    
    /// <summary>
    /// Asynchronously subscribe and handle a notification.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A Task</returns>
    /// <seealso cref="INotification"/>
    Task SubscribeAsync(TNotification notification, CancellationToken cancellationToken);
}