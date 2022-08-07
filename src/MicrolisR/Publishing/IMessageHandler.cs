namespace MicrolisR;

public interface IMessageHandler<in TMessage>
    where TMessage : IMessage
{
    Task SubscribeAsync(TMessage message, CancellationToken cancellationToken);
}

// public interface IMessageHandler<in TMessage> : IMessageHandler<TMessage, Unit>
//     where TMessage : IMessage<Unit>
// {
//     async Task<Unit> IMessageHandler<TMessage, Unit>.SubscribeAsync(TMessage message, CancellationToken cancellationToken)
//     {
//         await SubscribeAsync(message, cancellationToken);
//         return Unit.NewUnit;
//     }
//     
//     new Task SubscribeAsync(TMessage message, CancellationToken cancellationToken);
// }