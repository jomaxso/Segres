namespace Segres.AspNet;

public interface IHttpRequest : IRequest<IHttpResult>
{
}

public interface IHttpRequest<TResponse> : IRequest<IHttpResult<TResponse>>
{
}