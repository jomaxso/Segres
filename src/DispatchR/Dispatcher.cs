using System.Reflection;
using System.Runtime.CompilerServices;
using DispatchR.Contracts;

namespace DispatchR;

/// <inheritdoc />
public sealed class Dispatcher : IDispatcher
{
    private readonly ServiceResolver _serviceResolver;
    private readonly IHandlerCache<HandlerInfo> _queryHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _commandHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _streamHandlerCache;
    private readonly IHandlerCache<HandlerInfo[]> _eventHandlerCache;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(params Type[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(params Assembly[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="asSingleton"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(bool asSingleton = true, params Assembly[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="asSingleton"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(bool asSingleton = true, params Type[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public Dispatcher(IServiceProvider serviceProvider)
        : this(serviceProvider.GetService, Assembly.GetCallingAssembly())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(IServiceProvider serviceProvider, params Type[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(IServiceProvider serviceProvider, params Assembly[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    public Dispatcher(Func<Type, object?> serviceResolver)
        : this(serviceResolver, Assembly.GetCallingAssembly())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(Func<Type, object?> serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(Func<Type, object?> serviceResolver, params Assembly[] markers)
        : this(serviceResolver, markers.AsSpan())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Dispatcher(Func<Type, object?> serviceResolver, ReadOnlySpan<Assembly> markers)
        : this(new ServiceResolver(serviceResolver), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dispatcher"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    internal Dispatcher(ServiceResolver serviceResolver, ReadOnlySpan<Assembly> markers)
    {
        _serviceResolver = serviceResolver;
        _commandHandlerCache = markers.GetCommandHandlerDetails();
        _queryHandlerCache = markers.GetQueryHandlerDetails();
        _eventHandlerCache = markers.GetEventHandlerDetails();
        _streamHandlerCache = markers.GetStreamHandlerDetails();
    }

    #endregion

    /// <inheritdoc />
    public Task CommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResult> CommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var type = message.GetType();

        if (!_eventHandlerCache.TryGetValue(type, out var handlerTypes))
            return Task.CompletedTask;

        var handlers = new ReadOnlySpan<HandlerInfo>(handlerTypes);

        return handlers.Length switch
        {
            0 => Task.CompletedTask,
            1 => PublishSingleAsync(_serviceResolver, handlers[0], message, cancellationToken),
            _ => PublishMultipleAsync(_serviceResolver, handlers, message, cancellationToken)
        };
    }

    /// <inheritdoc />
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var requestType = query.GetType();
        var handlerInfo = _queryHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<QueryDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, query, cancellationToken);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TResult> StreamAsync<TResult>(IStream<TResult> stream, CancellationToken cancellationToken = default)
    {
        var requestType = stream.GetType();
        var handlerInfo = _streamHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<StreamDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, stream, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task PublishSingleAsync<TEvent>(ServiceResolver serviceResolver, HandlerInfo handlerInfo, TEvent message, CancellationToken cancellationToken)
        where TEvent : IEvent
    {
        var handler = serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle message of type: {message?.GetType().Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<EventDelegate>();
        return handlerDelegate.Invoke(handler, message, cancellationToken);
    }   
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task PublishMultipleAsync<TEvent>(ServiceResolver serviceResolver, ReadOnlySpan<HandlerInfo> handlerTypes, TEvent message, CancellationToken cancellationToken)
        where TEvent : IEvent
    {
        var length = handlerTypes.Length;
        var tasks = new Task[length];

        for (var i = 0; i < length; i++)
            tasks[i] = PublishSingleAsync(serviceResolver, handlerTypes[i], message, cancellationToken);

        return Task.WhenAll(tasks);
    }
}