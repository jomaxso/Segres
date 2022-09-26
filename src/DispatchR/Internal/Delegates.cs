using DispatchR.Contracts;

namespace DispatchR;

internal delegate object? ServiceResolver(Type type);
internal delegate Task MessageDelegate(object handler, IMessage message, CancellationToken cancellationToken);
internal delegate Task<T> QueryDelegate<T>(object handler, IQuery<T> query, CancellationToken cancellationToken);
internal delegate Task CommandDelegate(object handler, ICommand commandRequest, CancellationToken cancellationToken);
internal delegate Task<T> CommandDelegate<T>(object handler, ICommand<T> command, CancellationToken cancellationToken);


internal static class Delegates
{
    private const string MethodName = "HandleDynamicAsync"; 
    private static readonly Type DynamicMessageHandlerTypeWithOneParameter = typeof(DynamicMessageHandler<>);
    private static readonly Type DynamicCommandHandlerTypeWithOneParameter = typeof(DynamicCommandHandler<>);
    private static readonly Type DynamicCommandHandlerTypeWithTwoParameter = typeof(DynamicCommandHandler<,>);
    private static readonly Type DynamicQueryHandlerTypeWithTwoParameter = typeof(DynamicQueryHandler<,>);
    
    
    public static MessageDelegate CreateMessageDelegate(Type requestType)
    {
        return (MessageDelegate)Delegate.CreateDelegate(
            typeof(MessageDelegate), null, 
            DynamicMessageHandlerTypeWithOneParameter.MakeGenericType(requestType).GetMethod(MethodName)!);
    }
    
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