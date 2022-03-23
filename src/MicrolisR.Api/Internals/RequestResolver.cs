namespace MicrolisR.Api.Internals;

internal class RequestResolver
{
    private readonly Func<Type, object> _provider;
    public IDictionary<Type, Type> RequestEndpoints { get; }

    public RequestResolver(Func<Type, object> provider, IDictionary<Type, Type> requestEndpoints)
    {
        _provider = provider;
        RequestEndpoints = requestEndpoints;
    }

    public object? Resolve(Type key)
    {
        var handler = _provider.Invoke(RequestEndpoints[key]);
        return handler;
    }
}
