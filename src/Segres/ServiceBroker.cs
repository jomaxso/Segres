using System.Reflection;
using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;
using Segres.Internal;
using Segres.Internal.Cache;


namespace Segres;

/// <inheritdoc />
public sealed class ServiceBroker : IServiceBroker
{
    private readonly ServiceResolver _serviceResolver;
    private readonly Strategy _publishStrategy;

    private readonly IHandlerCache<HandlerInfo> _commandHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _streamHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _queryHandlerCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBroker"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public ServiceBroker(ServiceResolver serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).Distinct().ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBroker"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    /// <param name="publishStrategy">The strategy to publish events or messages.</param>
    public ServiceBroker(ServiceResolver serviceResolver, IEnumerable<Type> markers, Strategy publishStrategy = Strategy.WhenAll) 
        : this(serviceResolver, markers.Select(x => x.Assembly).ToArray().AsSpan(), publishStrategy)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBroker"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    /// <param name="publishStrategy">The strategy to publish events or messages.</param>
    public ServiceBroker(ServiceResolver serviceResolver, ReadOnlySpan<Assembly> markers, Strategy publishStrategy = Strategy.WhenAll)
    {
        _serviceResolver = serviceResolver;
        _publishStrategy = publishStrategy;
        _commandHandlerCache = markers.GetCommandHandlerDetails();
        _queryHandlerCache = markers.GetQueryHandlerDetails();
        _streamHandlerCache = markers.GetStreamHandlerDetails();
    }

    /// <inheritdoc />
    public Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage => await PublishAsync(message, _publishStrategy, cancellationToken);

    /// <inheritdoc />
    public ValueTask PublishAsync<TMessage>(TMessage message, Strategy strategy, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var handlerType = typeof(IEnumerable<IMessageHandler<TMessage>>);
        var handlers = (IMessageHandler<TMessage>[]) _serviceResolver.Invoke(handlerType);

        var length = handlers.Length;

        return length switch
        {
            0 => ValueTask.CompletedTask,
            1 => handlers[0].HandleAsync(message, cancellationToken),
            _ => strategy switch
            {
                Strategy.WhenAll => handlers.PublishWhenAll(length, message, cancellationToken),
                Strategy.WhenAny => handlers.PublishWhenAny(length, message, cancellationToken),
                _ => handlers.PublishSequential(message, cancellationToken)
            }
        };
    }

    /// <inheritdoc />
    public Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var requestType = query.GetType();
        var handlerInfo = _queryHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<QueryDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, query, cancellationToken);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStream<TResult> stream, CancellationToken cancellationToken = default)
    {
        var requestType = stream.GetType();
        var handlerInfo = _streamHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<StreamDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, stream, cancellationToken);
    }
}