using System.Runtime.CompilerServices;

namespace Segres.Internal.Cache;

internal static class HandlerCacheExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HandlerCache<TValue> ToHandlerCache<TSource, TValue>(this IEnumerable<KeyValuePair<Type, TSource>> source, Func<Type, TSource, TValue> elementSelector)
    {
        var context = new HandlerCache<TValue>();

        foreach (var x in source)
            context.Add(x.Key, elementSelector(x.Key, x.Value));

        return context;
    }
    
}