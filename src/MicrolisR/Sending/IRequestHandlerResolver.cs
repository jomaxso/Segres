namespace MicrolisR;

public interface IRequestHandlerResolver
{
    Task? ResolveAsync<TResponse>(object handler, IRequestable<TResponse> request,
        CancellationToken cancellationToken);
}