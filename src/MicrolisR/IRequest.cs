using MicrolisR.Validation;

namespace MicrolisR;


/// <summary>
/// Marker interface to represent a request with a response.
/// </summary>
/// <typeparam name="T">The response type</typeparam>
/// <seealso cref="IRequest"/>
public interface IRequest<T> : IValidatable
{
}

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="IRequest{T}"/>
public interface IRequest : IRequest<None>
{
}
