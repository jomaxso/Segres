using DispatchR.Contracts;

namespace DispatchR;

public delegate object ServiceResolver(Type type);

public delegate ValueTask StreamCallback<in TResult>(TResult streamItem);

public delegate ValueTask CancelableStreamCallback<in TResult>(TResult streamItem, CancellationToken cancellationToken);

internal delegate Task EventDelegate(object handler, IMessage message, CancellationToken cancellationToken);

internal delegate IAsyncEnumerable<T> StreamDelegate<T>(object handler, IStream<T> stream, CancellationToken cancellationToken);

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
    private static readonly Type DynamicStreamHandlerTypeWithTwoParameter = typeof(DynamicStreamHandler<,>);

    public static StreamDelegate<T> CreateStreamDelegate<T>(Type requestType)
        => CreateDelegate<StreamDelegate<T>>(DynamicStreamHandlerTypeWithTwoParameter, requestType, typeof(T));

    public static EventDelegate CreateEventDelegate(Type requestType)
        => CreateDelegate<EventDelegate>(DynamicMessageHandlerTypeWithOneParameter, requestType);

    public static QueryDelegate<T> CreateQueryDelegate<T>(Type requestType)
        => CreateDelegate<QueryDelegate<T>>(DynamicQueryHandlerTypeWithTwoParameter, requestType, typeof(T));

    public static CommandDelegate<T> CreateCommandDelegate<T>(Type requestType)
        => CreateDelegate<CommandDelegate<T>>(DynamicCommandHandlerTypeWithTwoParameter, requestType, typeof(T));

    public static CommandDelegate CreateCommandWithoutResponseDelegate(Type requestType)
        => CreateDelegate<CommandDelegate>(DynamicCommandHandlerTypeWithOneParameter, requestType);

    private static TDelegate CreateDelegate<TDelegate>(Type dynamicHandlerType, Type requestType)
        where TDelegate : Delegate => (TDelegate) dynamicHandlerType
        .MakeGenericType(requestType)
        .GetMethod(MethodName)!
        .CreateDelegate(typeof(TDelegate));

    private static TDelegate CreateDelegate<TDelegate>(Type dynamicHandlerType, Type requestType, Type responseType)
        where TDelegate : Delegate => (TDelegate) dynamicHandlerType
        .MakeGenericType(requestType, responseType)
        .GetMethod(MethodName)!
        .CreateDelegate(typeof(TDelegate));
}