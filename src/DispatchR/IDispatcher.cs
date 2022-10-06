using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Defines a mediator to encapsulate commands, events and queries as well as providing several common functionalities.
/// </summary>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TCommand}"/>
/// <seealso cref="IQuery{TResult}"/>
/// <seealso cref="IQueryHandler{TQuery,TResult}"/>
/// <seealso cref="ICommand{TResult}"/>
/// <seealso cref="ICommandHandler{TCommand,TResult}"/>
/// <seealso cref="IMessage"/>
/// <seealso cref="IMessageHandler{TEvent}"/>
public interface IDispatcher
{
    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    Task CommandAsync(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TCommand}"/>
    Task<TResult> CommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

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

    /// <summary>
    /// Asynchronously send a query to a single Receiver.
    /// </summary>
    /// <param name="query">The query object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TQuery,TResult}"/>
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamHandler{TStream,TResult}"/>
    IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStream<TResult> stream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="callback"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamHandler{TStream,TResult}"/>
    Task StreamAsync<TResult>(IStream<TResult> stream, StreamCallback<TResult> callback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="callback"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamHandler{TStream,TResult}"/>
    Task StreamAsync<TResult>(IStream<TResult> stream, CancelableStreamCallback<TResult> callback, CancellationToken cancellationToken = default);
}