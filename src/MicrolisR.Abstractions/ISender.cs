namespace MicrolisR.Abstractions;

public interface ISender
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
}