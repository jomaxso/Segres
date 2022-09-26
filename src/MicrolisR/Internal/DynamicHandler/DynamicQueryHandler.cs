namespace MicrolisR;

internal static class DynamicQueryHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    public static Task<TResponse>? HandleDynamicAsync(object obj, IQuery<TResponse> request, CancellationToken cancellationToken)
    {
        var handler = obj as IQueryHandler<TRequest, TResponse>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}