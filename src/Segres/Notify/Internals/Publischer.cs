namespace Segres;

internal sealed class Publisher : IPublisher
{
    private readonly ISubscriber _subscriber;

    public Publisher(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    /// <inheritdoc />
    public ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        return _subscriber.SubscribeAsync(notification, cancellationToken);
    }
}