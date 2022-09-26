namespace MicrolisR;

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="ICommand"/>.</typeparam>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommandHandler<in TRequest> 
    where TRequest : ICommand
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="ICommand"/>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a request.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="ICommand"/>.</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <seealso cref="ICommand{TResponse}"/>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommandHandler<in TRequest, TResponse> 
    where TRequest : ICommand<TResponse>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="ISender"/>
    /// <seealso cref="IQuery{T}"/>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}