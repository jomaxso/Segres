using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// Defines a service broker to encapsulate commands, events and queries as well as providing several common functionalities.
/// </summary>
/// <seealso cref="ISender"/>
/// <seealso cref="ICommand"/>
/// <seealso cref="ICommandHandler{TCommand}"/>
/// <seealso cref="IQuery{TResult}"/>
/// <seealso cref="IQueryHandler{TQuery,TResult}"/>
/// <seealso cref="ICommand{TResult}"/>
/// <seealso cref="ICommandHandler{TCommand,TResult}"/>
/// <seealso cref="IPublisher"/>
/// <seealso cref="IMessage"/>
/// <seealso cref="IMessageHandler{TEvent}"/>
/// <seealso cref="IStreamer"/>
/// <seealso cref="IStream{TResult}"/>
/// <seealso cref="IStreamHandler{TStream,TResult}"/>
public interface IServiceBroker : ISender, IPublisher, IStreamer
{
}