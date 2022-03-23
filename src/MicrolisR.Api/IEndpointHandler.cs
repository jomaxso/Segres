namespace MicrolisR.Api;

public interface IEndpointHandler
{
    Task<object?> HandleAsync(object request, CancellationToken cancellationToken = default);
}