namespace MicrolisR;



/// <summary>
/// Defines a subscriber for a notification. (just for internal usage) 
/// </summary>
/// <seealso cref="INotificationHandler{TNotification}"/>
public interface INotificationHandler
{
    internal Task HandleAsync<TNotification>(TNotification notification, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a subscriber for a notification.
/// </summary>
/// <seealso cref="INotification"/>
public interface INotificationHandler<in TNotification> : INotificationHandler
    where TNotification : INotification
{
    Task INotificationHandler.HandleAsync<T>(T notification, CancellationToken cancellationToken)
    {
        if (notification is TNotification subscribable)
            return HandleAsync(subscribable, cancellationToken);
        throw new ArgumentException($"The message is not of type {typeof(TNotification)}");
    }
    
    /// <summary>
    /// Asynchronously subscribe and handle a notification.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A Task</returns>
    /// <seealso cref="INotification"/>
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}