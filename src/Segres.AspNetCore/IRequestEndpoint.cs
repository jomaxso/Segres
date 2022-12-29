using Segres.Abstractions;

namespace Segres.AspNetCore;

public interface IRequestEndpoint<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IAsyncRequestEndpoint<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
}