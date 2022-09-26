using MicrolisR.Validation;

namespace MicrolisR;

/// <summary>
/// Defines a mediator to encapsulate request/response and publisher/subscriber patterns as well as providing several common functionalities related to these patterns.
/// </summary>
/// <seealso cref="ISender"/>
/// <seealso cref="IPublisher"/>
public interface IDispatcher : 
    IQuerySender,
    ICommandSender,
    IPublisher
{
}