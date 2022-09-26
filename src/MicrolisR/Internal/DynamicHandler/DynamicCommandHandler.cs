namespace MicrolisR;

internal static class DynamicCommandHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public static Task<TResponse>? HandleDynamicAsync(object obj, ICommand<TResponse> request, CancellationToken cancellationToken)
    {
        var handler = obj as ICommandHandler<TRequest, TResponse>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}

internal static class DynamicCommandHandler<TRequest>
    where TRequest : ICommand
{
    public static Task? HandleDynamicAsync(object obj, ICommand request, CancellationToken cancellationToken)
    {
        var handler = obj as ICommandHandler<TRequest>;
        return handler?.HandleAsync((TRequest) request, cancellationToken);
    }
}