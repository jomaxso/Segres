namespace Segres.Internal.Cache;

internal interface IHandlerCache<TValue> : IDictionary<Type, TValue>
{
    TValue FindHandler(Type key);
    
    TValue FindOrAddHandler(Type key, Func<Type, TValue> adding);
}