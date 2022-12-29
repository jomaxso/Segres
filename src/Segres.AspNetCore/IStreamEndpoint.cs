using Segres.Abstractions;

namespace Segres.AspNetCore;

public interface IStreamEndpoint<in TRequest, TResponse> : IAsyncRequestEndpoint<TRequest, IAsyncEnumerable<TResponse>>
    where TRequest : IStreamRequest<TResponse>
{
    /// <inheritdoc/>
    ValueTask<IAsyncEnumerable<TResponse>> IAsyncRequestHandler<TRequest, IAsyncEnumerable<TResponse>>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ValueTask.FromResult(HandleAsync(request, cancellationToken));

    /// <summary>
    /// Asynchronously receive and handle a stream request.
    /// </summary>
    /// <param name="request">The stream object</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the receive operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequest"/>
    new IAsyncEnumerable<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}