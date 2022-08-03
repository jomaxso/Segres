namespace MicrolisR;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequestable<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}


public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequestable
{
    async Task<Unit> IRequestHandler<TRequest, Unit>.HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return new Unit();
    }
    
    new Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}
