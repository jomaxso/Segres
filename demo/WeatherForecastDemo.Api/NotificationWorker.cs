public class NotificationWorker : BackgroundService
{
    private readonly OutboxPublisherContext? _publisherContext;

    public NotificationWorker(OutboxPublisherContext? publisherContext = null)
    {
        _publisherContext = publisherContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken);
            
            for (var i = 0; i < 20; i++)
                if (_publisherContext?.Db.TryDequeue(out var notification) is true)
                    await _publisherContext.ConsumeAsync(notification, stoppingToken);
        }
    }
}