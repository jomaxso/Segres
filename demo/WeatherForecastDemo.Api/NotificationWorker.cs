using Segres;
using Segres.Contracts;

public class MyPublisher : IPublisherContext
{
    public static Queue<INotification> Database { get; } = new Queue<INotification>();

    public ValueTask RaiseNotificationAsync(INotification notification, CancellationToken cancellationToken)
    {
        Database.Enqueue(notification);
        return ValueTask.CompletedTask;
    }
}

public class NotificationWorker : BackgroundService
{
    private readonly IConsumer _consumer;

    public NotificationWorker(IConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(20000, stoppingToken);
            
            for (var i = 0; i < 20; i++)
            {
                if (MyPublisher.Database.TryDequeue(out var notification))
                {
                    await _consumer.ConsumeAsync(notification, stoppingToken);
                }
            }
        }
    }
}