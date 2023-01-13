using Segres.Handlers;

namespace Segres.Contracts;

/// <summary>
/// Marker interface to represent a async request without a response.
/// </summary>
/// <seealso cref="IRequestHandler{TRequest}"/>
public interface IRequest : IRequest<None>
{
}

/// <summary>
/// Marker interface to represent a async request with a response.
/// </summary>
/// <seealso cref="IRequestHandler{TRequest}"/>
public interface IRequest<TResult>
{
}