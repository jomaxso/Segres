namespace DispatchR;

internal interface IHandlerCache<TValue> : IReadOnlyDictionary<Type, TValue>
{
    TValue FindHandler(Type key);
}