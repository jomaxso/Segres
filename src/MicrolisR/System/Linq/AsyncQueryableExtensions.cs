namespace System.Linq;

public static class AsyncQueryableExtensions
{
    public static async Task<List<TSource>> ToListAsync<TSource>(
        this IAsyncQueryable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();

        await foreach (var element in source.WithCancellation(cancellationToken))
            list.Add(element);

        return list;
    }

    public static async Task<TSource[]> ToArrayAsync<TSource>(
        this IAsyncQueryable<TSource> source,
        CancellationToken cancellationToken = default)
        => (await source.ToListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

    // public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncQueryable<TSource> asyncQueryable)
    //     => asyncQueryable as IAsyncEnumerable<TSource>;
}