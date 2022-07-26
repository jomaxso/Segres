namespace MicrolisR;

public interface ISender
{
    Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, CancellationToken cancellationToken = default);

    Task SendAsync(IRequestable request, CancellationToken cancellationToken = default);
}