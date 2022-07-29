namespace MicrolisR;

public interface IMediator : ISender
{
    Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, bool validate, CancellationToken cancellationToken = default);

    Task SendAsync(IRequestable request, bool validate, CancellationToken cancellationToken = default);
}