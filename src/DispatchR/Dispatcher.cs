using System.Reflection;
using DispatchR.Contracts;

namespace DispatchR;

/// <inheritdoc />
public sealed class Dispatcher : IDispatcher
{
    private readonly ServiceResolver _serviceResolver;
    private readonly IHandlerCache _requestHandlerCache;
    private readonly IDictionary<Type, Type[]> _messageHandlerDetails;

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
        _requestHandlerCache = markers.GetRequestHandlerDetails();
        _messageHandlerDetails = markers.GetSubscriberDetails();
    }

    #endregion

    /// <inheritdoc />
    public void Send(ICommand command)
        => SendAsync(command).Wait();

    /// <inheritdoc />
    public TResponse Send<TResponse>(ICommand<TResponse> command)
    {
        var response = SendAsync(command);

        response.Wait();
        return response.Result;
    }
    
    /// <inheritdoc />
    public Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveMethod<CommandDelegate>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");
        
        var handlerDelegate = handlerInfo.ResolveMethod<CommandDelegate<TResponse>>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse Send<TResponse>(IQuery<TResponse> query)
    {
        var response = SendAsync(query);
        
        response.Wait();
        return response.Result;
    }
    
    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var requestType = query.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveMethod<QueryDelegate<TResponse>>();
        return handlerDelegate.Invoke(handler, query, cancellationToken);
    }

    public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        => PublishAsync(message).Wait();

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : IMessage
    {
        var type = message.GetType();

        if (!_messageHandlerDetails.ContainsKey(type))
            throw new Exception($"No handler to handle request of type: {type.Name}");

        var handlerTypes = _messageHandlerDetails[type];

        for (var i = 0; i < handlerTypes.Length; i++)
        {
            var handlerType = handlerTypes[i];

            // _serviceResolver(handlerType)
            //     await handler.HandleAsync(message, cancellationToken).ConfigureAwait(false);
        }

        return Task.CompletedTask;
    }
}