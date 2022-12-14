using Microsoft.AspNetCore.Http;

namespace Segres.AspNet;

internal record struct EndpointRequest<TRequest>(TRequest Request) : IRequest<IResult>;