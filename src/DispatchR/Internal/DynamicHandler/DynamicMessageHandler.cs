using DispatchR.Contracts;

namespace DispatchR;

internal static class DynamicMessageHandler<TMessage>
    where TMessage : IMessage
{
    public static Task? HandleDynamicAsync(object obj, IMessage message, CancellationToken cancellationToken)
    {
        var handler = obj as IMessageHandler<TMessage>;
        return handler?.HandleAsync((TMessage) message, cancellationToken);
    }
}