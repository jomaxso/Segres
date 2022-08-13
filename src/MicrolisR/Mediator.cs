using System.Reflection;
using MicrolisR.Abstractions;

namespace MicrolisR;

/// <inheritdoc />
public sealed class Mediator : IMediator
{
    private readonly Func<Type, object?> _serviceResolver;
    private readonly IDictionary<Type, Type> _requestHandlerDetails;
    private readonly IDictionary<Type, Type[]> _messageHandlerDetails;

    #region Constructors
    
    public Mediator(params Type[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }
    
    public Mediator(params Assembly[] markers)
        : this(new DefaultProvider(true), markers)
    {
    }
    
    public Mediator(bool asSingleton = true, params Assembly[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }
    
    public Mediator(bool asSingleton = true, params Type[] markers)
        : this(new DefaultProvider(asSingleton), markers)
    {
    }
    
    public Mediator(IServiceProvider serviceProvider)
        : this(serviceProvider.GetService, Assembly.GetCallingAssembly())
    {
    }

    public Mediator(IServiceProvider serviceProvider, params Type[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    public Mediator(IServiceProvider serviceProvider, params Assembly[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    public Mediator(Func<Type, object?> serviceResolver)
        : this(serviceResolver, Assembly.GetCallingAssembly())
    {
    }

    public Mediator(Func<Type, object?> serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).ToArray())
    {
    }

    public Mediator(Func<Type, object?> serviceResolver, params Assembly[] markers)
    {
        _serviceResolver = serviceResolver;
        _requestHandlerDetails = markers.GetReceiverDetails();
        _messageHandlerDetails = markers.GetSubscriberDetails();
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
}