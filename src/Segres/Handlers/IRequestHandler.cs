using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// Defines a receiver (fire-and-forget) for a asyncRequest.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IAsyncRequestHandler{TRequest}"/>
public interface IRequestHandler<in TRequest> : IAsyncRequestHandler<TRequest>
    where TRequest : IRequest
{
    /// <inheritdoc/>
    ValueTask IAsyncRequestHandler<TRequest>.HandleAsync(TRequest request, CancellationToken cancellationToken)
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
    void Handle(TRequest request);
}

/// <summary>
/// Defines a receiver (fire-and-forget) for a asyncRequest.
/// </summary>
/// <typeparam name="TRequest">The request type witch implements <see cref="IRequest"/>.</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <seealso cref="IRequest"/>
/// <seealso cref="IAsyncRequestHandler{TRequest}"/>
public interface IRequestHandler<in TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc/>
    ValueTask<TResponse> IAsyncRequestHandler<TRequest, TResponse>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ValueTask.FromResult(Handle(request));

    /// <summary>
    /// Synchronously receive and handle a asyncRequest.
    /// </summary>
    /// <param name="request">The asyncRequest object</param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest"/>
    TResponse Handle(TRequest request);
}