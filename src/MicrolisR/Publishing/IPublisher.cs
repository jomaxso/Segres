namespace MicrolisR;

public interface IPublisher
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}