using MicrolisR.Abstractions;

namespace MicrolisR.Errors;

public static class SenderExtensions
{
    public static async ValueTask<Result<TResponse>> TrySendAsync<TResponse>(this ISender sender, IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sender.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return new Result<TResponse>(result);
        }
        catch (Exception e)
        {
            return new Result<TResponse>(e);
        }
    }
    
    public static async ValueTask<Result> TrySendAsync(this ISender sender, IRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await sender.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return Result.Empty;
        }
        catch (Exception e)
        {
            return new Result(e);
        }
    }
}