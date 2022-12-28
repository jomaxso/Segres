using Segres.Handlers;

namespace Segres.AspNet;

public interface IAsyncEndpoint<in TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
   public static abstract void Configure(EndpointDefinition endpoint);
}

public interface IAsyncEndpoint<in TRequest> : IAsyncEndpoint<TRequest, None>
    where TRequest : IHttpRequest
{
    async ValueTask<None> IAsyncRequestHandler<TRequest, None>.HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return None.Empty;
    }

    new ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken);
}