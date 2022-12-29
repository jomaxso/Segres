using Segres.Abstractions;

namespace Segres.AspNetCore;

public interface IAsyncRequestEndpoint<in TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public static abstract void Configure(IEndpointDefinition endpoint);
}