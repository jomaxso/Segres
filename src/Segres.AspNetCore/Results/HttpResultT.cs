using Microsoft.AspNetCore.Http;

namespace Segres.AspNetCore;

/// <inheritdoc />
public record HttpResult<T>(IResult Result) : IResult<HttpResult<T>, IResult>
{
    public static implicit operator HttpResult<T>(T value) => new(Results.Ok(value));


    internal HttpResult(T value) : this(Results.Ok(value))
    {
    }


    /// <inheritdoc />
    public static HttpResult<T> Create(IResult result)
    {
        return new HttpResult<T>(result);
    }

    internal static readonly HttpResult<T> NoContentResult = new(Results.NoContent());
    internal static readonly HttpResult<T> UnauthorizedResult = new(Results.Unauthorized());
}