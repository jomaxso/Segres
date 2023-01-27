namespace Segres;

internal sealed class DefaultPublisherContext : PublisherContext
{
    public DefaultPublisherContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override ValueTask OnPublishAsync(IEvent @event, CancellationToken cancellationToken)
        => ConsumeAsync(@event, cancellationToken);
}