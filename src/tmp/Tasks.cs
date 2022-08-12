namespace MicrolisR.Utilities;

internal static class Tasks
{
    public static async ValueTask AsValueTask<T>(this ValueTask<T> valueTask)
        => await valueTask;
    
    public static async Task<IEnumerable<T>> WhenAllAsync<T>(params Task<T>[] args)
    {
        var tasks = System.Threading.Tasks.Task.WhenAll(args);

        try
        {
            return await tasks;
        }
        catch (Exception)
        {
            // ignore
        }

        throw tasks.Exception!;
    }

    public static async Task WhenAllAsync(params Task[] args)
    {
        var tasks = System.Threading.Tasks.Task.WhenAll(args);

        try
        {
            await tasks;
        }
        catch (Exception)
        {
            // ignore
        }

        if (tasks.IsCompletedSuccessfully)
            return;
        
        throw tasks.Exception!;
    }

    public static Task<IEnumerable<T>> WhenAllAsync<T>(this IEnumerable<Task<T>> args)
        => WhenAllAsync(args.ToArray());

    public static Task WhenAllAsync(this IEnumerable<Task> args)
        => WhenAllAsync(args.ToArray());

    public static Task<IEnumerable<TResult>> WhenAllAsync<T, TResult>(this IEnumerable<T> values, Func<T, Task<TResult>> selector) 
        => values.Select(selector).WhenAllAsync();

    public static Task WhenAllAsync<T>(this IEnumerable<T> values, Func<T, Task> selector) 
        => values.Select(selector).WhenAllAsync();
}
