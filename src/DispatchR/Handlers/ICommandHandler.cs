using DispatchR.Contracts;

namespace DispatchR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TCommand">The request type witch implements <see cref="ICommand"/>.</typeparam>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TCommand}"/>
public interface ICommandHandler<in TCommand> 
    where TCommand : ICommand
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="ICommand"/>
    Task HandleAsync(TCommand request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TCommand">The request type witch implements <see cref="ICommand"/>.</typeparam>
/// <typeparam name="TResult"></typeparam>
/// <seealso cref="ICommand{TResponse}"/>
/// <seealso cref="ICommandHandler{TCommand}"/>
public interface ICommandHandler<in TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IQuery{T}"/>
    Task<TResult> HandleAsync(TCommand request, CancellationToken cancellationToken = default);
}