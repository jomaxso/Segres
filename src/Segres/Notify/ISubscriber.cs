namespace Segres;

public interface ISubscriber
{
    ValueTask SubscribeAsync(INotification notification, CancellationToken cancellationToken = default);
    ValueTask SubscribeAsync(INotification notification, bool asParallel, CancellationToken cancellationToken = default);
}