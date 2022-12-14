using System.Diagnostics;

namespace Segres;

internal class NotificationHandlerDefinition : IHandlerDefinition<NotificationHandlerDefinition>
{
    private NotificationHandlerDefinition(Type requestType)
    {
        var self = this.GetType();

        this.HandlerType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(INotificationHandler<>)
                .MakeGenericType(requestType));

        this.InvokeAsync = (Func<object[], INotification, bool, CancellationToken, ValueTask>) self
            .CreateInternalDelegate(nameof(CreateObserverDelegate), requestType);
    }

    public static NotificationHandlerDefinition Create(Type requestType)
        => new(requestType);

    public Type HandlerType { get; }

    public Func<object[], INotification, bool, CancellationToken, ValueTask> InvokeAsync { get; }

    private static Func<object[], INotification, bool, CancellationToken, ValueTask> CreateObserverDelegate<TNotification>()
        where TNotification : INotification
    {
        return (requestHandler, message, asParallel, cancellationToken) =>
        {
            if (requestHandler is not INotificationHandler<TNotification>[] handlers)
                throw new UnreachableException();

            var length = handlers.Length;

            return (length, asParallel) switch
            {
                (0, _) => ValueTask.CompletedTask,
                (1, _) => handlers[0].HandleAsync((TNotification) message, cancellationToken),
                (_, true) => PublishWhenAll(handlers, length, (TNotification) message, cancellationToken),
                (_, false) => PublishSequential(handlers, (TNotification) message, cancellationToken)
            };
        };
    }

    private static async ValueTask PublishWhenAll<TEvent>(INotificationHandler<TEvent>[] handlers, int length, TEvent message, CancellationToken cancellationToken)
        where TEvent : INotification
    {
        var tasks = new Task[length];
    
        for (var i = 0; i < length; i++)
            tasks[i] = handlers[i].HandleAsync(message, cancellationToken).AsTask();
        
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
            throw new Exception("One or more errors appeared while publishing notification " + all.Exception.InnerExceptions);
    }

    private static async ValueTask PublishSequential<TEvent>(IReadOnlyList<INotificationHandler<TEvent>> handlers, TEvent message, CancellationToken cancellationToken)
        where TEvent : INotification
    {
        for (var i = 0; i < handlers.Count; i++)
            await handlers[i].HandleAsync(message, cancellationToken);
    }
}