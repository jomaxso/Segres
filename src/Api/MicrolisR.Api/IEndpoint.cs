namespace MicrolisR.Api;

public interface IApiEndpoint<in TRequest, TResponse> : IEndpointHandler
{
    Task<TResponse?> HandleAsync(TRequest? request, CancellationToken cancellationToken = default);

    async Task<object?> IEndpointHandler.HandleAsync(object request, CancellationToken cancellationToken) => await HandleAsync((TRequest)request, cancellationToken);

}
