using System.Runtime.CompilerServices;
using Segres.Tmp.Http;

namespace Segres;

/// <summary>
/// Send a command or a query to be handled by a single handler.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="IRequestHandler{TRequest}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequestHandler{TRequest, TResult}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask<TResult> SendAsync<TResult>(IHttpRequest<TResult> request, CancellationToken cancellationToken = default);
    
}