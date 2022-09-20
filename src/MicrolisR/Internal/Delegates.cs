namespace MicrolisR;

internal delegate Task<T> QueryDelegate<T>(object handler, IQueryRequest<T> queryRequest, CancellationToken cancellationToken);
internal delegate Task CommandDelegate(object handler, ICommandRequest commandRequest, CancellationToken cancellationToken);
internal delegate Task<T> CommandDelegate<T>(object handler, ICommandRequest<T> commandRequest, CancellationToken cancellationToken);


internal static class Delegates
{
    private const string MethodName = "HandleDynamicAsync"; 
    private static readonly Type DynamicCommandHandlerTypeWithOneParameter = typeof(DynamicCommandHandler<>);
    private static readonly Type DynamicCommandHandlerTypeWithTwoParameter = typeof(DynamicCommandHandler<,>);
    private static readonly Type DynamicQueryHandlerTypeWithTwoParameter = typeof(DynamicQueryHandler<,>);
    
    public static QueryDelegate<T> CreateQueryDelegate<T>(Type requestType)
    {
        return (QueryDelegate<T>) Delegate.CreateDelegate(
            typeof(QueryDelegate<T>), null, 
            DynamicQueryHandlerTypeWithTwoParameter.MakeGenericType(requestType, typeof(T)).GetMethod(MethodName)!);
    }
    
    public static CommandDelegate<T> CreateCommandDelegate<T>(Type requestType)
    {
        return (CommandDelegate<T>) Delegate.CreateDelegate(
            typeof(CommandDelegate<T>), null, 
            DynamicCommandHandlerTypeWithTwoParameter.MakeGenericType(requestType, typeof(T)).GetMethod(MethodName)!);
    }

    public static CommandDelegate CreateCommandWithoutResponseDelegate(Type requestType)
    {
        return (CommandDelegate)Delegate.CreateDelegate(
            typeof(CommandDelegate), null, 
            DynamicCommandHandlerTypeWithOneParameter.MakeGenericType(requestType).GetMethod(MethodName)!);
    }
    
}