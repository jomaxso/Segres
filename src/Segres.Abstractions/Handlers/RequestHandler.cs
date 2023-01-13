using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a handler for a request implementing the <see cref="IRequest"/> interface
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IRequestHandler{TRequest}"/>
public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest
{
    /// <inheritdoc/>
    ValueTask IRequestHandler<TRequest>.HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        Handle(request);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Synchronously receive and handle a asyncRequest.
    /// </summary>
    /// <param name="request">The asyncRequest object</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest"/>
    protected abstract void Handle(TRequest request);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a asyncRequest.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IRequestHandler{TRequest}"/>
public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc/>
    ValueTask<TResponse> IRequestHandler<TRequest, TResponse>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ValueTask.FromResult(Handle(request));

    /// <summary>
    /// Synchronously receive and handle a asyncRequest.
    /// </summary>
    /// <param name="request">The asyncRequest object</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest"/>
    protected abstract TResponse Handle(TRequest request);
}