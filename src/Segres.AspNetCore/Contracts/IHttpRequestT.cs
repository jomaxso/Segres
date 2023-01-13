using System.Diagnostics.CodeAnalysis;
using Segres.Contracts;

namespace Segres.AspNetCore;

/// <summary>
/// Marker interface to represent a http request with a response.
/// </summary>
/// <seealso cref="AbstractEndpoint{TRequest,TResponse}"/>
public interface IHttpRequest<T> : IRequest<HttpResult<T>>
{
    /// <summary>
    /// Defines the route of the endpoint.
    /// </summary>
    [StringSyntax("Route")]
    public static abstract string RequestRoute { get; }

    /// <summary>
    /// Defines the type of the request to the endpoint.
    /// </summary>
    public static abstract RequestType RequestType { get; }
}