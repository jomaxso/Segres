namespace DispatchR;

internal sealed class HandlerCache<TValue> : Dictionary<Type, TValue>, IHandlerCache<TValue>
{
    public TValue FindHandler(Type key)
    {
        if (TryGetValue(key, out var value))
            return value;
        
        throw new InvalidOperationException($"No handler found for type: {key.Name}");
    }

    public void Add(KeyValuePair<Type, TValue> keyValuePair)
        => this.Add(keyValuePair.Key, keyValuePair.Value);
}