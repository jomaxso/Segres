namespace MicrolisR;

public interface IMediator : ISender, IPublisher
{
    Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, bool validate, CancellationToken cancellationToken = default);

    Task SendAsync(IRequestable request, bool validate, CancellationToken cancellationToken = default);

    Task PublishAsync(IMessage message, bool validate, CancellationToken cancellationToken = default);
}