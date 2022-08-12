namespace MicrolisR.Abstractions;

public interface ISubscriber
{
    internal Task SubscribeAsync<TNotification>(TNotification notification, CancellationToken cancellationToken);
}

public interface ISubscriber<in TNotification> : ISubscriber
    where TNotification : INotification
{
    Task ISubscriber.SubscribeAsync<T>(T notification, CancellationToken cancellationToken)
    {
        if (notification is TNotification subscribable)
            return SubscribeAsync(subscribable, cancellationToken);
        
        throw new ArgumentException($"The message is not of type {typeof(TNotification)}");
    }
    
    Task SubscribeAsync(TNotification notification, CancellationToken cancellationToken);
}