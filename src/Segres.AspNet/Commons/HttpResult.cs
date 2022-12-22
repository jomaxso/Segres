using Microsoft.AspNetCore.Http;

namespace Segres.AspNet;

public interface IHttpResult
{
    public Error? Error { get; }

    public bool IsSuccess => Error is null;
}


public interface IHttpResult<out TResponse>
{
    public TResponse? Result { get; }
    public Error? Error { get; }

    public bool IsSuccess => Result is not null && Error is null;
}

internal interface IAspNetResult
{
    IResult AspNetResult { get; }
}

internal readonly record struct OkHttpResult() : IHttpResult, IAspNetResult
{
    public IResult AspNetResult => Results.Ok();
    
    public Error? Error => null;
}

internal readonly record struct OkHttpResult<T>(T Result) : IHttpResult<T>, IAspNetResult
{
    public Error? Error => null;
    
    public IResult AspNetResult => Results.Ok(Result);
    
}


internal readonly record struct BadRequestHttpResult(Error? Error) : IHttpResult, IAspNetResult
{
    public IResult AspNetResult => Results.BadRequest(Error);
    
}

internal readonly record struct BadRequestHttpResult<T>(Error? Error) : IHttpResult<T>, IAspNetResult
{
    public T? Result => default;
    
    public IResult AspNetResult => Results.BadRequest(Error);
}