namespace DispatchR;

internal static class HandlerCacheExtensions
{
    public static HandlerCache ToHandlerCache<TSource>(this IEnumerable<KeyValuePair<Type, TSource>> source, Func<Type, TSource, HandlerInfo> elementSelector)
    {
        var context = new HandlerCache();

        foreach (var x in source)
            context.Add(x.Key, elementSelector(x.Key, x.Value));

        return context;
    }
    
}