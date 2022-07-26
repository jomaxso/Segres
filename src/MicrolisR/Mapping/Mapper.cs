using System.Diagnostics.CodeAnalysis;

namespace MicrolisR;

public class Mapper : IMapper
{
    private readonly IDictionary<Type, object> _handlers;
    private readonly IMapHandlerResolver[] _mapHandlerResolvers;

    public Mapper(Func<Type, object> serviceResolver, IDictionary<Type, Type> handlerDetails)
    {
        _handlers = handlerDetails
            .Select(x => new KeyValuePair<Type, object>(x.Key, serviceResolver(x.Value)))
            .ToDictionary(x => x.Key, y => y.Value);
        _mapHandlerResolvers = FindAllHandlerResolver<IMapHandlerResolver>(handlerDetails);
    }

    public TResponse Map<TResponse>(IMappable<TResponse> request)
    {
        var requestType = request.GetType();
        
        if (!_handlers.ContainsKey(requestType))
            throw new Exception("No handler found form mapping the request");
        
        var handler = _handlers[requestType];

        for (var i = 0; i < _mapHandlerResolvers.Length; i++)
        {
            var result = _mapHandlerResolvers[i].Resolve(handler, request);

            if (result is not null)
                return result;
        }

        throw new Exception("No Mapper found");
    }
    
    private static T[] FindAllHandlerResolver<T>(IDictionary<Type, Type> details)
    {
        var requestHandlerResolvers = new List<T>();

        foreach (var value in details.Values)
        {
            var resolvers = value.Assembly
                .GetTypes()
                .Where(x => x.GetInterface(typeof(T).Name) is not null)
                .Select(Activator.CreateInstance)
                .Where(x => x is not null)
                .Cast<T>();

            requestHandlerResolvers.AddRange(resolvers);
        }

        return requestHandlerResolvers.DistinctBy(x => x!.GetType()).ToArray();
    }
}