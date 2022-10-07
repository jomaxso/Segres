using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// Defines a mediator to encapsulate commands, events and queries as well as providing several common functionalities.
/// </summary>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TCommand}"/>
/// <seealso cref="IQuery{TResult}"/>
/// <seealso cref="IQueryHandler{TQuery,TResult}"/>
/// <seealso cref="ICommand{TResult}"/>
/// <seealso cref="ICommandHandler{TCommand,TResult}"/>
/// <seealso cref="IMessage"/>
/// <seealso cref="IMessageHandler{TEvent}"/>
public interface IMediator : ISender, IPublisher, IStreamer
{
}