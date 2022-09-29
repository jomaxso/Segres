// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using DispatchR.Benchmarks;

BenchmarkRunner.Run<Benchmarks>();


// await stream.StartAsync(HandleItem);
//
// static Task HandleItem(int item, Action cancel)
// {
//     return Task.CompletedTask;
// }
//
// public static class AsyncEnumerableExtension
// {
//     public static async Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
//     {
//         var list = new List<TSource>();
//
//         await foreach (var item in source.WithCancellation(cancellationToken))
//         {
//             if (cancellationToken.IsCancellationRequested)
//                 break;
//
//             list.Add(item);
//         }
//
//         return list;
//     }
//
//     public static async IAsyncEnumerable<TTarget> Select<TSource, TTarget>(
//         this IAsyncEnumerable<TSource> source,
//         Func<TSource, TTarget> predicate,
//         [EnumeratorCancellation] CancellationToken cancellationToken = default)
//     {
//         await foreach (var item in source.WithCancellation(cancellationToken))
//         {
//             if (cancellationToken.IsCancellationRequested)
//                 break;
//
//             yield return predicate.Invoke(item);
//         }
//     }
//
//     public static async IAsyncEnumerable<TSource> Where<TSource>(
//         this IAsyncEnumerable<TSource> source,
//         Func<TSource, bool> predicate,
//         [EnumeratorCancellation] CancellationToken cancellationToken = default)
//     {
//         await foreach (var item in source.WithCancellation(cancellationToken))
//         {
//             if (cancellationToken.IsCancellationRequested)
//                 break;
//
//             if (predicate.Invoke(item))
//                 yield return item;
//         }
//     }
//
//     public static async ValueTask StartAsync<T>(this IAsyncEnumerable<T> stream, Func<T, Action, Task> callback, CancellationToken cancellationToken = default)
//     {
//         var tokenSource = new CancellationTokenSource();
//
//         await foreach (var current in stream.WithCancellation(tokenSource.Token))
//         {
//             if (cancellationToken.IsCancellationRequested)
//                 tokenSource.Cancel();
//
//             if (tokenSource.IsCancellationRequested)
//                 break;
//
//             await callback(current, tokenSource.Cancel);
//         }
//     }
// }