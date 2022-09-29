using System.Runtime.CompilerServices;

namespace DispatchR.Extensions;

public static class AsyncEnumerableExtentions
{
    public static async Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, int maxSize, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();

        var tokenSource = new CancellationTokenSource();

        await foreach (var current in source.WithCancellation(tokenSource.Token))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tokenSource.Cancel();
                break;
            }

            list.Add(current);

            if (list.Count >= maxSize)
                tokenSource.Cancel();
        }

        return list;
    }

    public static async Task<TSource[]> ToArrayAsync<TSource>(this IAsyncEnumerable<TSource> source, int size, CancellationToken cancellationToken = default)
    {
        var index = 0;
        var array = new TSource[size];
        var tokenSource = new CancellationTokenSource();
        
        await foreach (var current in source.WithCancellation(tokenSource.Token))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tokenSource.Cancel();
                break;
            }

            array[index] = current;
            index += 1;
            
            if (index >= size)
                tokenSource.Cancel();
        }

        return array;
    }

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

    public static async ValueTask StartAsync<T>(this IAsyncEnumerable<T> stream, Func<T, Action, Task> callback, CancellationToken cancellationToken = default)
    {
        var tokenSource = new CancellationTokenSource();

        await foreach (var current in stream.WithCancellation(tokenSource.Token))
        {
            if (cancellationToken.IsCancellationRequested)
                tokenSource.Cancel();

            if (tokenSource.IsCancellationRequested)
                break;

            await callback(current, tokenSource.Cancel);
        }
    }
}