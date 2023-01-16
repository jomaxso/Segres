namespace Segres.AspNetCore;

/// <summary>
/// Marker interface to represent a http request without a response.
/// </summary>
/// <seealso cref="AbstractEndpoint{TRequest}"/>
public interface IHttpRequest : IHttpRequest<None>
{
}