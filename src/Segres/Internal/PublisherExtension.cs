using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;
using Segres.Internal.Cache;

namespace Segres.Internal;

internal static class PublisherExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static async ValueTask PublishWhenAll<TMessage>(this IMessageHandler<TMessage>[] handlers, int length, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var tasks = new Task[length];

        for (var i = 0; i < length; i++)
            tasks[i] = handlers[i].HandleAsync(message, cancellationToken).AsTask();

        // return Task.WhenAll(tasks);
        var all = Task.WhenAll(tasks);

        try
        {
            await all;
            return;
        }
        catch (Exception)
        {
            // ignored
        }

        if (all.Exception is not null)
            throw new Exception("One or more errors appeared while publishing message " + all.Exception.InnerExceptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static async ValueTask PublishWhenAny<TMessage>(this IMessageHandler<TMessage>[] handlers, int length, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var tasks = new Task[length];

        for (var i = 0; i < length; i++)
            tasks[i] = handlers[i].HandleAsync(message, cancellationToken).AsTask();

        // return Task.WhenAny(tasks);
        var any = Task.WhenAny(tasks);

        try
        {
            await any;
            return;
        }
        catch (Exception)
        {
            // ignored
        }

        if (any.Exception is not null)
            throw new Exception("One or more errors appeared while publishing message " + any.Exception.InnerExceptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static async ValueTask PublishSequential<TMessage>(this IMessageHandler<TMessage>[] handlers, TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        for (var i = 0; i < handlers.Length; i++)
            await handlers[i].HandleAsync(message, cancellationToken);
    }
}