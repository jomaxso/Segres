namespace Segres;

internal sealed class DefaultPublisherContext : PublisherContext
{
    public DefaultPublisherContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override ValueTask OnPublishAsync(INotification notification, CancellationToken cancellationToken)
        => ConsumeAsync(notification, cancellationToken);
}