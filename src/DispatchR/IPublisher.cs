﻿using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Publish a message or event to be handled by multiple subscribers.
/// </summary>
/// <seealso cref="IMessage"/>
/// <seealso cref="IMessageHandler{TNotification}"/>
public interface IPublisher
{
    /// <summary>
    /// Send a notification to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <returns>A task that represents the publish operation.</returns>
    void Publish<TMessage>(TMessage message)
        where TMessage : IMessage;

    /// <summary>
    /// Asynchronously send a notification to multiple subscribers.
    /// </summary>
    /// <param name="message">The message object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage;
}