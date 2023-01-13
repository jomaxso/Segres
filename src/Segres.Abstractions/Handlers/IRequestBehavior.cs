using Segres.Contracts;

namespace Segres.Handlers;

/// <summary>
/// The delegate for asynchronously intercepting a request with the <see cref="IRequestBehavior{TRequest,TResult}"/>. 
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate ValueTask<TResult> RequestDelegate<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);

/// <summary>
/// A interceptor for a request using the <see cref="ISender"/> to call the matching <see cref="IRequestHandler{TRequest}"/> or <see cref="IRequestHandler{TRequest}"/>
/// </summary>
/// <typeparam name="TRequest">Th request type. Has to implement <see cref="IRequest{TResult}"/>.</typeparam>
/// <typeparam name="TResult">The type of the result specified from the <see cref="IRequest{TResult}"/>.</typeparam>
public interface IRequestBehavior<in TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    /// <summary>
    /// Hook into the pipeline of a request before calling the request handler or the next interceptor.
    /// </summary>
    /// <param name="next">The next step of the pipeline.</param>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An cancellation token.</param>
    /// <returns>A task that represents the receive operation. The task contains the result of this interception.</returns>
    public ValueTask<TResult> HandleAsync(RequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken);
}