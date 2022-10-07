using Segres.Contracts;

namespace Segres;

public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a message to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage;

    /// <summary>
    /// Asynchronously send a message to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="strategy">The publish strategy how the message has to be processed.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TMessage>(TMessage message, Strategy strategy, CancellationToken cancellationToken = default)
        where TMessage : IMessage;
}