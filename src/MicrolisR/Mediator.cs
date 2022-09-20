using System.Reflection;
using MicrolisR.Pipelines;

namespace MicrolisR;

/// <inheritdoc />
public sealed class Mediator : IMediator
{
    private readonly Func<Type, object?> _serviceResolver;
    private readonly IDictionary<Type, (Type Type, Delegate Del)> _requestHandlerDetails;
    private readonly IDictionary<Type, Type[]> _messageHandlerDetails;
    private readonly IDictionary<Type, Type[]> _pipelineStepsDetails;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(params Type[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(params Assembly[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="asSingleton"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(bool asSingleton = true, params Assembly[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="asSingleton"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(bool asSingleton = true, params Type[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public Mediator(IServiceProvider serviceProvider)
        : this(serviceProvider.GetService, Assembly.GetCallingAssembly())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(IServiceProvider serviceProvider, params Type[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(IServiceProvider serviceProvider, params Assembly[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    public Mediator(Func<Type, object?> serviceResolver)
        : this(serviceResolver, Assembly.GetCallingAssembly())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(Func<Type, object?> serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(Func<Type, object?> serviceResolver, params Assembly[] markers)
    {
        _serviceResolver = serviceResolver;
        _requestHandlerDetails = markers.GetRequestHandlerDetails();
        _messageHandlerDetails = markers.GetSubscriberDetails();
        _pipelineStepsDetails = markers.GetPipelineDetails();
        // _validator = serviceResolver(typeof(IValidator)) as IValidator;
    }

    #endregion

    #region Sender

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        if (!_requestHandlerDetails.ContainsKey(requestType))
            throw new Exception($"No handler to handle request of type: {requestType.Name}");

        var handlerInfo = _requestHandlerDetails[requestType];

        var handler = _serviceResolver(handlerInfo.Type) 
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return ((CommandDelegate<TResponse>)handlerInfo.Del).Invoke(handler, request, cancellationToken)!;
    }

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        // var pipelines = GetPipelines(requestType);
        //
        // var length = pipelines.Count;
        //
        // for (var i = 0; i < length; i++)
        // {
        //     request = await (Task<IRequest<TResponse>>) pipelines[i].BeforeAsync(request, cancellationToken);
        // }

        if (!_requestHandlerDetails.ContainsKey(requestType))
            throw new Exception($"No handler to handle request of type: {requestType.Name}");

        var handlerInfo = _requestHandlerDetails[requestType];

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return ((QueryDelegate<TResponse>)handlerInfo.Del).Invoke(handler, request, cancellationToken);

        // for (var i = 0; i < length; i++)
        // {
        //     response = await ((Task<TResponse>) pipelines[i].AfterAsync(response, cancellationToken)).ConfigureAwait(false);
        // }
    }

    /// <inheritdoc />
    public Task SendAsync(ICommandRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        if (!_requestHandlerDetails.ContainsKey(requestType))
            throw new Exception($"No handler to handle request of type: {requestType.Name}");

        var handlerInfo = _requestHandlerDetails[requestType];

        var handler = _serviceResolver(handlerInfo.Type) 
               ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return ((CommandDelegate)handlerInfo.Del).Invoke(handler, request, cancellationToken)!;
    }

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

    # endregion

    # region Publisher

    /// <inheritdoc />
    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
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
}