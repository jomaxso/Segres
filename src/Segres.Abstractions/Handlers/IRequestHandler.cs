using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a handler for a request implementing the <see cref="IRequest"/> interface.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <seealso cref="IRequest"/>
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, None>
    where TRequest : IRequest
{
    /// <inheritdoc />
    async ValueTask<None> IRequestHandler<TRequest, None>.HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return None.Empty;
    }

    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object implementing <see cref="IRequest"/>.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation.</returns>
    /// <seealso cref="IRequest"/>
   new ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a request implementing the <see cref="IRequest{TResponse}"/> interface.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest{TResult}"/>.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
/// <seealso cref="IRequest{TResponse}"/>
public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object implementing <see cref="IRequest{TResponse}"/>.</param>
    /// <param name="cancellationToken">An cancellation token</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest{TResponse}"/>
    ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}