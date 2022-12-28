using System.Runtime.CompilerServices;
using Segres.Contracts;

namespace Segres;

/// <summary>
/// Publish a notification or event to be handled by multiple handlers.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a notification to multiple subscribers.
    /// </summary>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}