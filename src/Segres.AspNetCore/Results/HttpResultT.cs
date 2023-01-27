using Microsoft.AspNetCore.Http;

namespace Segres.AspNetCore;

/// <inheritdoc />
public record HttpResult<T>(IResult Result) : IHttpResult<HttpResult<T>, IResult>
{
    public static implicit operator HttpResult<T>(T value) => new(Results.Ok(value));
    public static implicit operator HttpResult<T>(Result<T> result) => new(result.IsSuccess ? Results.Ok(result.GetValue()) : Results.BadRequest(result.GetError()));


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
    
    public IResult GetValue()
    {
        return Result;
    }
}