using MicrolisR.Validation;

namespace MicrolisR;

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandRequestHandler{T}"/>
public interface ICommandRequest : IValidatable
{
}

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandRequestHandler{T}"/>
public interface ICommandRequest<TResponse> : IValidatable
{
}