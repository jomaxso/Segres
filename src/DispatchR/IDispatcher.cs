namespace DispatchR;

/// <summary>
/// Defines a mediator to encapsulate request/response and publisher/subscriber patterns as well as providing several common functionalities related to these patterns.
/// </summary>
/// <seealso cref="ISender"/>
public interface IDispatcher : ISender, IPublisher
{
}