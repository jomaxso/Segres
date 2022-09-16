namespace MicrolisR;

internal static class DynamicCommandHandler<TRequest, TResponse>
    where TRequest : ICommandRequest<TResponse>
{
    public static Task<TResponse>? HandleDynamicAsync(object obj, ICommandRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handler = obj as ICommandRequestHandler<TRequest, TResponse>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}

internal static class DynamicCommandHandler<TRequest>
    where TRequest : ICommandRequest
{
    public static Task? HandleDynamicAsync(object obj, ICommandRequest request, CancellationToken cancellationToken)
    {
        var handler = obj as ICommandRequestHandler<TRequest>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}