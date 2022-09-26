namespace DispatchR;

internal static class HandlerCacheExtensions
{
    public static HandlerCache<TValue> ToHandlerCache<TSource, TValue>(this IEnumerable<KeyValuePair<Type, TSource>> source, Func<Type, TSource, TValue> elementSelector)
    {
        var context = new HandlerCache<TValue>();

        foreach (var x in source)
            context.Add(x.Key, elementSelector(x.Key, x.Value));

        return context;
    }
    
}