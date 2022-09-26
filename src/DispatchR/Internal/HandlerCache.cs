namespace DispatchR;

// internal sealed class HandlerCache : Dictionary<Type, HandlerInfo>, IHandlerCache<HandlerInfo>
// {
//     public HandlerInfo FindHandler(Type key)
//     {
//         if (!this.ContainsKey(key))
//             throw new InvalidOperationException($"No handler found for type: {key.Name}");
//
//         return this[key];
//     }
//
//     public HandlerInfo FindHandler<T>(T _) => FindHandler(typeof(T));
//
//     public void Add(KeyValuePair<Type, HandlerInfo> keyValuePair) 
//         => this.Add(keyValuePair.Key, keyValuePair.Value);
// }

internal sealed class HandlerCache<TValue> : Dictionary<Type, TValue>, IHandlerCache<TValue>
{
    public TValue FindHandler(Type key)
    {
        if (!this.ContainsKey(key))
            throw new InvalidOperationException($"No handler found for type: {key.Name}");

        return this[key];
    }

    public bool TryFindHandler(Type key, out TValue? value)
    {
        var isFound = this.ContainsKey(key);
        value = isFound ? this[key] : default;
        return isFound;
    }

    public TValue FindHandler<T>(T _) => FindHandler(typeof(T));

    public void Add(KeyValuePair<Type, TValue> keyValuePair)
        => this.Add(keyValuePair.Key, keyValuePair.Value);
}