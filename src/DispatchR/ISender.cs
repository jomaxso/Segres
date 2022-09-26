using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Send a command to be handled by a single handler.
/// </summary>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ISender
{
    /// <summary>
    /// Send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    void Send(ICommand command);
    
    /// <summary>
    /// Send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TRequest,TResponse}"/>
    TResponse Send<TResponse>(ICommand<TResponse> command);

    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="command">The command object</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send a query to a single Receiver.
    /// </summary>
    /// <param name="query">The query object</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TRequest,TResponse}"/>
    TResponse Send<TResponse>(IQuery<TResponse> query);
    
    /// <summary>
    /// Asynchronously send a query to a single Receiver.
    /// </summary>
    /// <param name="query">The query object</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQueryHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}