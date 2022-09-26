namespace MicrolisR;

internal interface IHandlerCache
{
    HandlerInfo FindHandler(Type key);
    HandlerInfo FindHandler<T>(T _);

    void Add(KeyValuePair<Type, HandlerInfo> keyValuePair);
    void Add(Type key, HandlerInfo value);
}

internal sealed class HandlerCache : Dictionary<Type, HandlerInfo>, IHandlerCache
{
    public HandlerInfo FindHandler(Type key)
    {
        if (!this.ContainsKey(key))
            throw new InvalidOperationException($"No handler found for type: {key.Name}");

        return this[key];
    }

    public HandlerInfo FindHandler<T>(T _) => FindHandler(typeof(T));

    public void Add(KeyValuePair<Type, HandlerInfo> keyValuePair) 
        => this.Add(keyValuePair.Key, keyValuePair.Value);
}