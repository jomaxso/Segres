namespace MicrolisR.Abstractions;

public interface IPublisher
{
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}