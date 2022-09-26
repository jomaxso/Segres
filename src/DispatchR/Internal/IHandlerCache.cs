namespace DispatchR;

internal interface IHandlerCache
{
    HandlerInfo FindHandler(Type key);
    HandlerInfo FindHandler<T>(T _);

    void Add(KeyValuePair<Type, HandlerInfo> keyValuePair);
    void Add(Type key, HandlerInfo value);
}