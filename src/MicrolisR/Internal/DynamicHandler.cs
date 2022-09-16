namespace MicrolisR;

public static class DynamicHandler
{
    private const string MethodName = "HandleDynamicAsync"; 
    private static readonly Type DynamicCommandHandlerType_1 = typeof(DynamicCommandHandler<>);
    private static readonly Type DynamicCommandHandlerType_2 = typeof(DynamicCommandHandler<,>);
    private static readonly Type DynamicQueryHandlerType_2 = typeof(DynamicQueryHandler<,>);
    
    public static Delegate CreateQueryDelegate<T>(Type requestType)
    {
        return Delegate.CreateDelegate(
            typeof(Func<object, IQueryRequest<T>, CancellationToken, Task<T>?>),
            null, DynamicQueryHandlerType_2.MakeGenericType(requestType, typeof(T)).GetMethod(MethodName)!);
    }
    
    public static Delegate CreateCommandDelegater<T>(Type requestType)
    {
        return Delegate.CreateDelegate(
            typeof(Func<object, ICommandRequest<T>, CancellationToken, Task<T>?>),
            null, DynamicCommandHandlerType_2.MakeGenericType(requestType, typeof(T)).GetMethod(MethodName)!);
    }
    
    public static Delegate CreateCommandDelegate(Type requestType)
    {
        return Delegate.CreateDelegate(
            typeof(Func<object, ICommandRequest, CancellationToken, Task?>),
            null, DynamicCommandHandlerType_1.MakeGenericType(requestType).GetMethod(MethodName)!);
    }

    public static Task<TResponse>? InvokeQueryHandler<TResponse>(this Delegate del, object handler, IQueryRequest<TResponse> request, CancellationToken cancellationToken)
    {
        if (del is Func<object, IQueryRequest<TResponse>, CancellationToken, Task<TResponse>?> func)
            return func.Invoke(handler, request, cancellationToken);

        return null;
    }

    public static Task<TResponse>? InvokeCommandHandler<TResponse>(this Delegate del, object handler, ICommandRequest<TResponse> request, CancellationToken cancellationToken)
    {
        if (del is Func<object, ICommandRequest<TResponse>, CancellationToken, Task<TResponse>?> func)
            return func.Invoke(handler, request, cancellationToken);

        return null;
    }

    public static Task? InvokeCommandHandler(this Delegate del, object handler, ICommandRequest request, CancellationToken cancellationToken)
    {
        if (del is Func<object, ICommandRequest, CancellationToken, Task?> func)
            return func.Invoke(handler, request, cancellationToken);

        return null;
    }
}