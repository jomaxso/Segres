using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// Send a request to be handled by a single handler.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IRequestHandler{TRequest}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}