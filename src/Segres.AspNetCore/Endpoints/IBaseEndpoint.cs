using Microsoft.AspNetCore.Builder;
using Segres.Handlers;

namespace Segres.AspNetCore;

internal interface IBaseEndpoint<in TRequest, TResponse> : IRequestHandler<TRequest, HttpResult<TResponse>>
    where TRequest : IHttpRequest<TResponse>
{
    void Configure(RouteHandlerBuilder routeBuilder);
}