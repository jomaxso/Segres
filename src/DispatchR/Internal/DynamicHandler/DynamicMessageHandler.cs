using DispatchR.Contracts;

namespace DispatchR;

internal static class DynamicMessageHandler<TMessage>
    where TMessage : IEvent
{
    public static Task? HandleDynamicAsync(object obj, IEvent message, CancellationToken cancellationToken)
    {
        var handler = obj as IEventHandler<TMessage>;
        return handler?.HandleAsync((TMessage) message, cancellationToken);
    }
}