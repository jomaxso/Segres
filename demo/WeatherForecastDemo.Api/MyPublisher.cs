using System.Collections.Concurrent;
using Segres.Abstractions;

public sealed class MyPublisher : IPublisher
{
    public static readonly ConcurrentQueue<INotification> Notifications = new();

    public ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
        where TNotification : INotification
    {
        Notifications.Enqueue(notification);
        return ValueTask.CompletedTask;
    }
}

