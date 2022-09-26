using MicrolisR.Validation;

namespace MicrolisR;

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommand : IValidatable
{
}

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommand<TResponse> : IValidatable
{
}