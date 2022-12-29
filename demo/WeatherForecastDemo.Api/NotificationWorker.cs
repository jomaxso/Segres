using Segres.Abstractions;

public class NotificationWorker : BackgroundService
{
    private readonly ISubscriber _subscriber;

    public NotificationWorker(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(20000, stoppingToken);
            
            for (var i = 0; i < 20; i++)
            {
                if (MyPublisher.Notifications.TryDequeue(out var notification))
                {
                    await _subscriber.SubscribeAsync(notification, stoppingToken);
                }
            }
        }
    }
}