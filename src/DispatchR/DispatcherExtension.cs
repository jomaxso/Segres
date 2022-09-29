using System.Runtime.CompilerServices;
using DispatchR.Contracts;

namespace DispatchR;

internal static class DispatcherExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Task CorePublishAsync<TMessage>(this ServiceResolver serviceResolver, HandlerInfo[] handlerInfos, TMessage message, Strategy strategy,
        CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        return handlerInfos.Length switch
        {
            0 => Task.CompletedTask,
            1 => PublishSingleAsync(serviceResolver, handlerInfos[0], message, cancellationToken),
            _ => PublishMultipleAsync(serviceResolver, handlerInfos, strategy, message, cancellationToken)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task PublishSingleAsync<TMessage>(ServiceResolver serviceResolver, HandlerInfo handlerInfo, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var handler = serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle message of type: {message?.GetType().Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<EventDelegate>();
        return handlerDelegate.Invoke(handler, message, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task PublishMultipleAsync<TMessage>(ServiceResolver serviceResolver, IReadOnlyList<HandlerInfo> handlerInfos, Strategy strategy, TMessage message,
        CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        return strategy switch
        {
            Strategy.Default => BlockedPublishAsync(serviceResolver, handlerInfos, message, cancellationToken),
            Strategy.WhenAll => WhenAllPublishAsync(serviceResolver, handlerInfos, message, cancellationToken),
            Strategy.WhenAny => WhenAnyPublishAsync(serviceResolver, handlerInfos, message, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task[] CreatePublishTasks<TMessage>(ServiceResolver serviceResolver, IReadOnlyList<HandlerInfo> handlerInfos, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var length = handlerInfos.Count;
        var tasks = new Task[length];

        for (var i = 0; i < length; i++)
            tasks[i] = PublishSingleAsync(serviceResolver, handlerInfos[i], message, cancellationToken);

        return tasks;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task BlockedPublishAsync<TMessage>(ServiceResolver serviceResolver, IReadOnlyList<HandlerInfo> handlerInfos, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var length = handlerInfos.Count;
        for (var i = 0; i < length; i++)
            await PublishSingleAsync(serviceResolver, handlerInfos[i], message, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task WhenAllPublishAsync<TMessage>(ServiceResolver serviceResolver, IReadOnlyList<HandlerInfo> handlerInfos, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var tasks = CreatePublishTasks(serviceResolver, handlerInfos, message, cancellationToken);
        var all = Task.WhenAll(tasks);

        try
        {
            await all.ConfigureAwait(false);
            return;
        }
        catch (Exception)
        {
            // ignored
        }

        if (all.Exception is not null)
            throw new Exception("One or more errors appeared while publishing message" + all.Exception.InnerExceptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task WhenAnyPublishAsync<TMessage>(ServiceResolver serviceResolver, IReadOnlyList<HandlerInfo> handlerInfos, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var tasks = CreatePublishTasks(serviceResolver, handlerInfos, message, cancellationToken);
        var any = Task.WhenAny(tasks);

        try
        {
            await any.ConfigureAwait(false);
            return;
        }
        catch (Exception)
        {
            // ignored
        }

        if (any.Exception is not null)
            throw new Exception("One or more errors appeared while publishing message" + any.Exception.InnerExceptions);
    }
}