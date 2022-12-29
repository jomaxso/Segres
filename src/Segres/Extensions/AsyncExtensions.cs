namespace Segres;

internal static class AsyncExtensions
{
    internal static TResult Await<TResult>(this ValueTask<TResult> valueTask)
    {
        if (valueTask.IsCompleted)
            return valueTask.Result;

        return valueTask.AsTask()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    internal static void Await(this ValueTask valueTask)
    {
        if (valueTask.IsCompleted is false)
        {
            valueTask.AsTask()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}