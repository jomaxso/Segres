using Microsoft.AspNetCore.Http;

namespace Segres.AspNetCore;


/// <inheritdoc cref="HttpResult{None}"/>
public record HttpResult : HttpResult<None>, IHttpResult<HttpResult, IResult>
{
    internal static readonly HttpResult OkResult = new(Results.Ok());
    internal new static readonly HttpResult NoContentResult = new(Results.NoContent());
    internal new static readonly HttpResult UnauthorizedResult = new(Results.Unauthorized());
    
    public static implicit operator HttpResult(Result<None> result) => new(result.IsSuccess ? Results.Ok() : Results.BadRequest(result.GetError()));


    internal HttpResult(IResult result) : base(result)
    {
    }

    /// <inheritdoc />
    public new static HttpResult Create(IResult result)
    {
        return new HttpResult(result);
    }
}