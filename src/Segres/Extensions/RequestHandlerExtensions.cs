using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

internal static class RequestHandlerExtensions
{
    public static ValueTask<TResponse> ExecuteRequestHandler<TRequest, TResponse>(this IRequestHandler<TRequest, TResponse> handler, IRequest<TResponse> r, CancellationToken c)
        where TRequest : IRequest<TResponse> => handler.HandleAsync((TRequest) r, c);
}