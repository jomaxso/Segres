using Segres.Contracts;
using Segres.Handlers;

namespace Segres.Internal.DynamicHandler;

internal static class DynamicQueryHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    public static Task<TResponse>? HandleDynamicAsync(object obj, IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        var handler = obj as IQueryHandler<TRequest, TResponse>;
        return handler?.HandleAsync((TRequest) query, cancellationToken);
    }
}