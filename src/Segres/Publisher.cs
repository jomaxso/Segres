using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <inheritdoc />
internal sealed class Publisher : IPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PublishStrategy _publishStrategy;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="publishStrategy">The strategy to publish events or messages.</param>
    public Publisher(IServiceProvider serviceProvider, PublishStrategy publishStrategy = PublishStrategy.WhenAll)
    {
        _serviceProvider = serviceProvider;
        _publishStrategy = publishStrategy;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : INotification => PublishAsync(message, _publishStrategy, cancellationToken);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask PublishAsync<TMessage>(TMessage message, PublishStrategy strategy, CancellationToken cancellationToken = default)
        where TMessage : INotification
    {
        if (_serviceProvider.GetServices<INotificationHandler<TMessage>>() is not INotificationHandler<TMessage>[] handlers)
            return ValueTask.CompletedTask;
        
        var length = handlers.Length;

        return length switch
        {
            0 => ValueTask.CompletedTask,
            1 => handlers[0].HandleAsync(message, cancellationToken),
            _ => strategy switch
            {
                PublishStrategy.WhenAll => PublishWhenAll(handlers, length, message, cancellationToken),
                PublishStrategy.WhenAny => PublishWhenAny(handlers, length, message, cancellationToken),
                _ => PublishSequential(handlers, message, cancellationToken)
            }
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask PublishWhenAll<TMessage>(IReadOnlyList<INotificationHandler<TMessage>> handlers, int length, TMessage message, CancellationToken cancellationToken)
        where TMessage : INotification
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
    private static async ValueTask PublishWhenAny<TMessage>(IReadOnlyList<INotificationHandler<TMessage>> handlers, int length, TMessage message, CancellationToken cancellationToken)
        where TMessage : INotification
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
    private static async ValueTask PublishSequential<TMessage>(IReadOnlyList<INotificationHandler<TMessage>> handlers, TMessage message, CancellationToken cancellationToken)
        where TMessage : INotification
    {
        for (var i = 0; i < handlers.Count; i++)
            await handlers[i].HandleAsync(message, cancellationToken);
    }
}