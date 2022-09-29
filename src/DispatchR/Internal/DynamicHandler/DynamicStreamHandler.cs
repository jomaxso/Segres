using DispatchR.Contracts;

namespace DispatchR;

internal static class DynamicStreamHandler<TStream, TResult>
    where TStream : IStream<TResult>
{
    public static IAsyncEnumerable<TResult>? HandleDynamicAsync(object obj, IStream<TResult> request, CancellationToken cancellationToken)
    {
        var handler = obj as IStreamHandler<TStream, TResult>;
        return handler?.HandleAsync((TStream) request, cancellationToken);
    }
}