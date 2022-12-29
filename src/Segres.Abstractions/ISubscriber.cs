namespace Segres.Abstractions;

/// <summary>
/// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="IAsyncNotificationHandler{TNotification}"/>.
/// </summary>
public interface ISubscriber
{
    /// <summary>
    /// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="IAsyncNotificationHandler{TNotification}"/>.
    /// </summary>
    /// <param name="notification">The request object to consume.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    ValueTask SubscribeAsync(INotification notification, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="IAsyncNotificationHandler{TNotification}"/>.
    /// </summary>
    /// <param name="notification">The request object to consume.</param>
    /// <param name="asParallel"></param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    ValueTask SubscribeAsync(INotification notification, bool asParallel, CancellationToken cancellationToken = default);
}