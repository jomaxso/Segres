using System.Runtime;

namespace MicrolisR;

public sealed class Sender : ISender
{
    private readonly Func<Type, object> _serviceResolver;
    private readonly IDictionary<Type, Type> _handlerDetails;
    private readonly IRequestHandlerResolver[] _requestHandlerResolvers;

    public Sender(Func<Type, object> serviceResolver, IDictionary<Type, Type> handlerDetails)
    {
        _serviceResolver = serviceResolver;
        _handlerDetails = handlerDetails;
        _requestHandlerResolvers = FindAllHandlerResolver<IRequestHandlerResolver>(handlerDetails);
    }

    public Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var handler = GetHandler(request);

        for (var i = 0; i < _requestHandlerResolvers.Length; i++)
        {
            var task = _requestHandlerResolvers[i].ResolveAsync(handler, request, cancellationToken);

            if (task is Task<TResponse> responseTask)
                return responseTask;
        }

        throw new AmbiguousImplementationException($"No RequestHandlerResolver found for type {request.GetType().Name}");
    }


    public async Task SendAsync(IRequestable request, CancellationToken cancellationToken = default)
    {
        var handler = GetHandler(request);
        
        for (var i = 0; i < _requestHandlerResolvers.Length; i++)
        {
            var task = _requestHandlerResolvers[i].ResolveAsync(handler, request, cancellationToken);

            if (task is null)
                continue;

            await task;
            return;
        }

        throw new AmbiguousImplementationException($"No RequestHandlerResolver found for type {request.GetType().Name}");
    }

    private object GetHandler<TResponse>(IRequestable<TResponse> request)
    {
        var requestType = request.GetType();

        if (!_handlerDetails.ContainsKey(requestType))
            throw new Exception($"No handler to handle request of type: {requestType.Name}");

        var handlerType = _handlerDetails[requestType];
        return _serviceResolver(handlerType);
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