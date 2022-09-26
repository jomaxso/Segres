using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Publish a message or event to be handled by multiple subscribers.
/// </summary>
/// <seealso cref="IMessage"/>
/// <seealso cref="IMessageHandler{TNotification}"/>
public interface IPublisher
{
    /// <summary>
    /// Send a message to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <returns>A task that represents the publish operation.</returns>
    void Publish<TMessage>(TMessage message)
        where TMessage : IMessage;

    /// <summary>
    /// Asynchronously send a message to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage;
}