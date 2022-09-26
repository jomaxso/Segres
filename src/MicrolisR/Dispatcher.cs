using System.Reflection;
using MicrolisR.Pipelines;

namespace MicrolisR;

/// <inheritdoc />
public sealed class Dispatcher : IDispatcher
{
    private readonly ServiceResolver _serviceResolver;
    private readonly IHandlerCache _requestHandlerCache;
    private readonly IDictionary<Type, Type[]> _messageHandlerDetails;
    private readonly IDictionary<Type, Type[]> _pipelineStepsDetails;

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
        _pipelineStepsDetails = markers.GetPipelineDetails();
        // _validator = serviceResolver(typeof(IValidator)) as IValidator;
    }

    #endregion

    #region Sender

    /// <inheritdoc />
    public void Send(ICommand request)
        => SendAsync(request).Wait();

    /// <inheritdoc />
    public TResponse Send<TResponse>(ICommand<TResponse> request)
    {
        var response = SendAsync(request);

        response.Wait();

        return response.Result;
    }
    
    /// <inheritdoc />
    public Task SendAsync(ICommand request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveMethod<CommandDelegate>();
        return handlerDelegate.Invoke(handler, request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");
        
        var handlerDelegate = handlerInfo.ResolveMethod<CommandDelegate<TResponse>>();
        return handlerDelegate.Invoke(handler, request, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse Send<TResponse>(IQuery<TResponse> request)
    {
        var response = SendAsync(request);

        response.Wait();

        return response.Result;
    }
    
    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerInfo = _requestHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveMethod<QueryDelegate<TResponse>>();
        return handlerDelegate.Invoke(handler, request, cancellationToken);
    }

    # endregion

    # region Publisher

    /// <inheritdoc />
    public void Publish<TNotification>(TNotification notification) 
        where TNotification : INotification
        => PublishAsync(notification)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    
    
    /// <inheritdoc />
    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var type = notification.GetType();

        if (!_messageHandlerDetails.ContainsKey(type))
            throw new Exception($"No handler to handle request of type: {type.Name}");

        var handlerTypes = _messageHandlerDetails[type];

        for (var i = 0; i < handlerTypes.Length; i++)
        {
            var handlerType = handlerTypes[i];

            if (_serviceResolver(handlerType) is INotificationHandler handler)
                await handler.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }
    }
    
    # endregion
    
    private IList<IPipelineBehavior> GetPipelines(Type requestType)
    {
        if (!_pipelineStepsDetails.ContainsKey(requestType))
            return Array.Empty<IPipelineBehavior>();

        var pipelineTypes = _pipelineStepsDetails[requestType];

        var pipelineBehaviors = new IPipelineBehavior[pipelineTypes.Length];

        for (var i = 0; i < pipelineTypes.Length; i++)
        {
            var pipeline = _serviceResolver(pipelineTypes[i]) as IPipelineBehavior;

            if (pipeline is null)
                continue;

            pipelineBehaviors[i] = pipeline;
        }

        return pipelineBehaviors;
    }
}