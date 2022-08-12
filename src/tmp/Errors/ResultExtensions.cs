namespace MicrolisR.Errors;

public static class ResultExtensions
{
    public static async ValueTask<TResult> ThrowIfErrorAsync<TResult>(this ValueTask<Result<TResult>> task) => await task;
    
    public static async ValueTask<TResult> MatchAsync<TResult>(this ValueTask<Result<TResult>> task, Func<TResult?, Error?, TResult> onHandle)
    {
        var result = await task;
        return result.Match(onHandle);
    }
    
    public static async ValueTask<TResult> MatchAsync<TResult>(this ValueTask<Result<TResult>> task, Func<Error, TResult> onError)
    {
        var result = await task;
        return result.Match(onError);
    }

    public static async ValueTask<TResult> MatchAsync<TResult>(this ValueTask<Result<TResult>> task, Func<TResult, TResult> onValue, Func<Error, TResult> onError)
    {
        var result = await task;
        return result.Match(onValue, onError);
    }
    
    public static TResult Match<TResult>(this Result<TResult> result, Func<TResult?, Error?, TResult> onHandle)
        => onHandle(result.Value!, result.Error) ;
    
    public static TResult Match<TResult>(this Result<TResult> result, Func<Error, TResult> onError)
        => result.Error is not null ? onError((Error) result.Error) : result.Value!;
    
    public static TResult Match<TValue, TResult>(this Result<TValue> result, Func<TValue, TResult> onValue, Func<Error, TResult> onError)
        => result.Error is not null ? onError((Error) result.Error) : onValue(result.Value!);
}