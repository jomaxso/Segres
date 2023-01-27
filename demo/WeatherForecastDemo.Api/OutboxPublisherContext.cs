using Segres;

public class OutboxPublisherContext : PublisherContext
{
    private static Queue<IEvent> Database { get; } = new();

    public Queue<IEvent> Db => Database;

    public OutboxPublisherContext(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }
    
    public override ValueTask OnPublishAsync(IEvent @event, CancellationToken cancellationToken)
    {
        Database.Enqueue(@event);
        return ValueTask.CompletedTask;
    }
}