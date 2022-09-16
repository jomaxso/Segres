namespace MicrolisR;

internal static class DynamicQueryHandler<TRequest, TResponse>
    where TRequest : IQueryRequest<TResponse>
{
    public static Task<TResponse>? HandleDynamicAsync(object obj, IQueryRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handler = obj as IQueryRequestHandler<TRequest, TResponse>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}