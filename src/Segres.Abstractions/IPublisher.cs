using System.Runtime.CompilerServices;

namespace Segres;

/// <summary>
/// Publish a event to be handled by multiple handlers.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a event to multiple subscribers.
    /// </summary>
    /// <param name="message">The event object.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    /// Synchronously send a event to multiple subscribers.
    /// </summary>
    /// <param name="message">The event object</param>
    /// <returns>A task that represents the publish operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Publish<TEvent>(TEvent message)
        where TEvent : IEvent;
}