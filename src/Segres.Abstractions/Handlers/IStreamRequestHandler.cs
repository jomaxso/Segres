namespace Segres;

/// <inheritdoc />
public interface IStreamRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, IAsyncEnumerable<TResponse>>
    where TRequest : IStreamRequest<TResponse>
{
    ValueTask<IAsyncEnumerable<TResponse>> IRequestHandler<TRequest, IAsyncEnumerable<TResponse>>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ValueTask.FromResult(HandleAsync(request, cancellationToken));

    /// <summary>
    /// Asynchronously receive and handle a request.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    new IAsyncEnumerable<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}



