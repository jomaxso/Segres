namespace Segres;

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="IRequestHandler{TRequest}"/>
public interface IRequest : IRequest<None>
{
}

/// <summary>
/// Marker interface to represent a request with a response.
/// </summary>
/// <seealso cref="IRequestHandler{TRequest, TResult}"/>
public interface IRequest<TResult>
{
}