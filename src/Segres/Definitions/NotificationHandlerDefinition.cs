using System.Diagnostics;

namespace Segres;

internal class NotificationHandlerDefinition : IHandlerDefinition<NotificationHandlerDefinition>
{
    private NotificationHandlerDefinition(Type requestType)
    {
        var self = this.GetType();

        this.HandlerType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(IEventHandler<>)
                .MakeGenericType(requestType));

        this.InvokeAsync = (Func<object[], IEvent, CancellationToken, ValueTask>) self
            .CreateInternalDelegate(nameof(CreateObserverDelegate), requestType);
    }

    public static NotificationHandlerDefinition Create(Type requestType)
        => new(requestType);

    public Type HandlerType { get; }

    public Func<object[], IEvent, CancellationToken, ValueTask> InvokeAsync { get; }

    private static Func<object[], IEvent, CancellationToken, ValueTask> CreateObserverDelegate<TEvent>()
        where TEvent : IEvent
    {
        return async (requestHandler, message, cancellationToken) =>
        {
            if (requestHandler is IEventHandler<TEvent>[] handlers)
            {
                var length = handlers.Length;

                for (var i = 0; i < length; i++)
                    await handlers[i].HandleAsync((TEvent)message, cancellationToken);
            }
        };
    }
}