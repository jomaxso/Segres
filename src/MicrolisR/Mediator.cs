using System.Reflection;
using MicrolisR.Validation;

namespace MicrolisR;

/// <inheritdoc />
public sealed class Mediator : IMediator
{
    private readonly IValidator? _validator;
    private readonly Func<Type, object?> _serviceResolver;
    private readonly IDictionary<Type, Type> _requestHandlerDetails;
    private readonly IDictionary<Type, Type[]> _messageHandlerDetails;

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
        _requestHandlerDetails = markers.GetReceiverDetails();
        _messageHandlerDetails = markers.GetSubscriberDetails();
        _validator = serviceResolver(typeof(IValidator)) as IValidator;
    }

    #endregion

    #region Sender

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handler = GetRequestHandler(requestType);

        return (Task<TResponse>) handler.ReceiveAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handler = GetRequestHandler(requestType);

        return handler.ReceiveAsync(request, cancellationToken);
    }

    private IReceiver GetRequestHandler(Type requestType)
    {
        if (!_requestHandlerDetails.ContainsKey(requestType))
            throw new Exception($"No handler to handle request of type: {requestType.Name}");

        var handlerType = _requestHandlerDetails[requestType];

        return _serviceResolver(handlerType) as IReceiver
               ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");
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
            var handler = _serviceResolver(handlerType) as ISubscriber;
            if (handler is null)
                continue;

            await handler.SubscribeAsync(notification, cancellationToken).ConfigureAwait(false);
        }
    }

    # endregion

    /// <inheritdoc />
    public void Validate<T>(T value) where T : IValidatable
        => _validator?.Validate(value);
}