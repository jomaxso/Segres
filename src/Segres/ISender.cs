using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// 
/// </summary>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TCommand}"/>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously send a query to a single Receiver.
    /// </summary>
    /// <param name="query">The query object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TQuery,TResult}"/>
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}