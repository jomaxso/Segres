using Segres;

public class OutboxPublisherContext : PublisherContext
{
    private static Queue<INotification> Database { get; } = new();

    public Queue<INotification> Db => Database;

    public OutboxPublisherContext(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }
    
    public override ValueTask OnPublishAsync(INotification notification, CancellationToken cancellationToken)
    {
        Database.Enqueue(notification);
        return ValueTask.CompletedTask;
    }
}