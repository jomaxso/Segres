using System.Runtime.CompilerServices;

namespace Segres.Extensions;

public static class AsyncEnumerableExtentions
{
    public static async IAsyncEnumerable<TTarget> Select<TSource, TTarget>(this IAsyncEnumerable<TSource> source, Func<TSource, TTarget> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var item in source.WithCancellation(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            yield return predicate.Invoke(item);
        }
    }

    public static async IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Predicate<TSource> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var item in source.WithCancellation(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                break;
    
            if (predicate.Invoke(item))
                yield return item;
        }
    }
}