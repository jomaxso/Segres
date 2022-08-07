using MicrolisR.Utilities;

namespace MicrolisR;

public class Publisher : IPublisher
{
    private readonly Func<Type, object?> _serviceResolver;
    private readonly IDictionary<Type, Type[]> _handlerDetails;
    private readonly IMessageHandlerResolver[] _messageHandlerResolvers;


    public Publisher(Func<Type, object?> serviceResolver, IDictionary<Type, Type[]> handlerDetails)
    {
        _serviceResolver = serviceResolver;

        _handlerDetails = handlerDetails;
        _messageHandlerResolvers = FindAllHandlerResolver<IMessageHandlerResolver>(handlerDetails).ToArray();
    }

    public Task PublishAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        var handlers = GetHandlers(message);

        return handlers.WhenAllAsync(handler =>
        {
            return _messageHandlerResolvers
                .Select(handlerResolver => handlerResolver.ResolveAsync(handler, message, cancellationToken))
                .WhenAllAsync();
        });
    }

    private IEnumerable<object> GetHandlers(IMessage message)
    {
        var type = message.GetType();

        if (!_handlerDetails.ContainsKey(type))
            throw new Exception($"No handler to handle request of type: {type.Name}");

        var handlerTypes = _handlerDetails[type];

        var handlers = new object?[handlerTypes.Length];

        for (var i = 0; i < handlerTypes.Length; i++)
        {
            var handlerType = handlerTypes[i];
            handlers[i] = _serviceResolver(handlerType);
        }

        return handlers.Where(x => x is not null).ToArray()!;
    }

    private static IEnumerable<T> FindAllHandlerResolver<T>(IDictionary<Type, Type[]> details)
    {
        var requestHandlerResolvers = new List<T>();

        foreach (var detail in details)
        {
            foreach (var handler in detail.Value)
            {
                var resolvers = handler.Assembly
                    .GetTypes()
                    .Where(x => x.GetInterface(typeof(T).Name) is not null)
                    .Select(Activator.CreateInstance)
                    .Where(x => x is not null)
                    .Cast<T>();

                requestHandlerResolvers.AddRange(resolvers);
            }
        }

        return requestHandlerResolvers.DistinctBy(x => x!.GetType()).ToArray();
    }
}