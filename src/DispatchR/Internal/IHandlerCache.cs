namespace DispatchR;

internal interface IHandlerCache<TValue>
{
    TValue FindHandler(Type key);
    TValue FindHandler<T>(T _);
    bool TryFindHandler(Type key, out TValue? value);

    void Add(KeyValuePair<Type, TValue> keyValuePair);
    void Add(Type key, TValue value);
}