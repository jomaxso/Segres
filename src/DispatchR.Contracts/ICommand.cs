namespace DispatchR.Contracts;

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommand
{
}

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TRequest}"/>
public interface ICommand<TResponse>
{
}