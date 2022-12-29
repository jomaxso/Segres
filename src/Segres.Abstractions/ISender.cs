using System.Runtime.CompilerServices;

namespace Segres.Abstractions;

/// <summary>
/// Send a command or a query to be handled by a single handler.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Synchronously send a request to a single handler. 
    /// </summary>
    /// <param name="request">The request object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Send(IRequest request);

    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="IAsyncRequestHandler{TRequest}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default);
  

    /// <summary>
    /// Synchronously send a request to a single handler. 
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The result of the executed handler.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    TResponse Send<TResponse>(IRequest<TResponse> request);

    /// <summary>
    /// Asynchronously send a asyncRequest to a single handler.
    /// </summary>
    /// <param name="request">The asyncRequest object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IAsyncRequestHandler{TRequest}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);


    /// <summary>
    /// Synchronously send a stream to a single handler. 
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The result of the executed handler.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IAsyncEnumerable<TResponse> Send<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A streamRequest as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamRequestHandler{TRequest,TResponse}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<IAsyncEnumerable<TResult>> SendAsync<TResult>(IStreamRequest<TResult> request, CancellationToken cancellationToken = default)
        => SendAsync((IRequest<IAsyncEnumerable<TResult>>)request, cancellationToken);
}