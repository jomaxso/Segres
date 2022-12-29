namespace Segres.Abstractions;

/// <inheritdoc />
public interface IStreamRequestHandler<in TRequest, TResponse> : IAsyncRequestHandler<TRequest, IAsyncEnumerable<TResponse>>
    where TRequest : IStreamRequest<TResponse>
{
    ValueTask<IAsyncEnumerable<TResponse>> IAsyncRequestHandler<TRequest, IAsyncEnumerable<TResponse>>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        => ValueTask.FromResult(HandleAsync(request, cancellationToken));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>A stream as <see cref="IAsyncEnumerable{T}"/>.</returns>
    new IAsyncEnumerable<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}



