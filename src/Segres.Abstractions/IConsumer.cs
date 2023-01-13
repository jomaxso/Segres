using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="INotificationHandler{TNotification}"/>.
/// </summary>
public interface IConsumer
{
    /// <summary>
    /// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="INotificationHandler{TNotification}"/>.
    /// </summary>
    /// <param name="notification">The request object to consume.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    ValueTask ConsumeAsync(INotification notification, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Consumes a <see cref="INotification"/> and sends it to the registered <see cref="INotificationHandler{TNotification}"/> or <see cref="INotificationHandler{TNotification}"/>.
    /// </summary>
    /// <param name="notification">The request object to consume.</param>
    /// <param name="asParallel"></param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    ValueTask ConsumeAsync(INotification notification, bool asParallel, CancellationToken cancellationToken = default);
}