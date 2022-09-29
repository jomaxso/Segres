namespace DispatchR.Contracts;

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TCommand}"/>
public interface ICommand
{
}

/// <summary>
/// Marker interface to represent a request without a response.
/// </summary>
/// <seealso cref="ICommandHandler{TCommand}"/>
public interface ICommand<TResult>
{
}