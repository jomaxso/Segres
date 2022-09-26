namespace MicrolisR;

/// <summary>
/// Send a command to be handled by a single handler.
/// </summary>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommandSender
{
    /// <summary>
    /// Send a command to a single Receiver.
    /// </summary>
    /// <param name="request">The command object</param>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    void Send(ICommand request);
    
    /// <summary>
    /// Send a command to a single Receiver.
    /// </summary>
    /// <param name="request">The command object</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TRequest,TResponse}"/>
    TResponse Send<TResponse>(ICommand<TResponse> request);

    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="request">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="ICommandHandler{TRequest}"/>
    Task SendAsync(ICommand request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously send a command to a single Receiver.
    /// </summary>
    /// <param name="request">The command object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="ICommandHandler{TRequest,TResponse}"/>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
}