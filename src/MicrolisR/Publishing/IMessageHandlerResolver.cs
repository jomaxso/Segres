namespace MicrolisR;

public interface IMessageHandlerResolver
{
    Task ResolveAsync(object handler, IMessage message,
        CancellationToken cancellationToken);
}