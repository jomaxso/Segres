using Segres.Contracts;

namespace Segres.AspNet;

public interface IHttpRequest : IHttpRequest<None>
{
}

public interface IHttpRequest<TResponse> :  IRequest<TResponse> //  IRequest<IHttpResult<TResponse>>,
{
}