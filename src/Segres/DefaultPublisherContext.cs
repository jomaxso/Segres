using Segres.Contracts;

namespace Segres;

internal sealed class DefaultPublisherContext : IPublisherContext
{
    private readonly IConsumer _consumer;

    public DefaultPublisherContext(IConsumer consumer)
    {
        _consumer = consumer;
    }

    public ValueTask RaiseNotificationAsync(INotification notification, CancellationToken cancellationToken)
        => _consumer.ConsumeAsync(notification, cancellationToken);
}