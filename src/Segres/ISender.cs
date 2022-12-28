using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

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
    ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default);

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
}