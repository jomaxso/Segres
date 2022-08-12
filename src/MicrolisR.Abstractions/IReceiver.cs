namespace MicrolisR.Abstractions;

public interface IReceiver
{
    internal Task ReceiverAsync<T>(T request, CancellationToken cancellationToken);
}

public interface IReceiver<in TRequest, TResponse> : IReceiver
    where TRequest : IRequest<TResponse>
{
    Task IReceiver.ReceiverAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return ReceiverAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }

    Task<TResponse> ReceiverAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IReceiver<in TRequest> : IReceiver<TRequest, Unit>
    where TRequest : IRequest
{
    Task IReceiver.ReceiverAsync<T>(T request, CancellationToken cancellationToken)
    {
        if (request is TRequest requestable)
            return ReceiverAsync(requestable, cancellationToken);

        throw new ArgumentException($"The request is not of type {typeof(TRequest)}");
    }
    
    async Task<Unit> IReceiver<TRequest, Unit>.ReceiverAsync(TRequest request, CancellationToken cancellationToken)
    {
        await ReceiverAsync(request, cancellationToken).ConfigureAwait(false);
        return Unit.NewUnit;
    }

    new Task ReceiverAsync(TRequest request, CancellationToken cancellationToken);
}