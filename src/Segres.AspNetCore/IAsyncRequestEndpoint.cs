using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Segres.Abstractions;

namespace Segres.AspNetCore;

public interface IAsyncRequestEndpoint<TRequest, TResponse> : IAsyncRequestHandler<EndpointRequest<TRequest, TResponse>, IResult>
    where TRequest : IRequest<TResponse>
{
    public static abstract void Configure(IEndpointDefinition endpoint);

    async ValueTask<IResult> IAsyncRequestHandler<EndpointRequest<TRequest, TResponse>, IResult>.HandleAsync(
        EndpointRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken)
    {
        var response = await ResolveAsync(request.Request, cancellationToken);
        return response.Result;
    }

    ValueTask<IEndpointResult<TResponse>> ResolveAsync(TRequest request, CancellationToken cancellationToken);
}

public abstract class AbstractEndpoint<TRequest, TResponse> : IAsyncRequestHandler<EndpointRequest<TRequest, TResponse>, IResult>
    where TRequest : IRequest<TResponse>
{
    async ValueTask<IResult> IAsyncRequestHandler<EndpointRequest<TRequest, TResponse>, IResult>.HandleAsync(
        EndpointRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken)
    {
        var response = await ResolveAsync(request.Request, cancellationToken);
        return response.Result;
    }

    protected abstract ValueTask<IEndpointResult<TResponse>> ResolveAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IEndpointResult
{
    public IResult Result { get; }
}

public interface IEndpointResult<T> : IEndpointResult
{
}

public readonly record struct EndpointResult(IResult Result) : IEndpointResult
{
    public static IEndpointResult Ok() => Ok<object>();
    public static IEndpointResult Accepted(string? uri = null, object? value = null) => Accepted<object>(uri, value);
    public static IEndpointResult Created(Uri uri, object? value = null) => Created<object>(uri, value);
    public static IEndpointResult Created(string uri, object? value = null) => Created<object>(uri, value);
    public static IEndpointResult CreatedAtRoute(string? routeName = null, object? routeValues = null, object? value = null) => CreatedAtRoute<object>(routeName, routeValues, value);
    public static IEndpointResult NoContent() => NoContent<object>();
    public static IEndpointResult BadRequest(object? error = null) => BadRequest<object>(error);
    public static IEndpointResult Unauthorized() => Unauthorized<object>();
    public static IEndpointResult Forbidden(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null) => Forbidden<object>(properties, authenticationSchemes);
    public static IEndpointResult NotFound(object? error = null) => NotFound<object>(error);
    public static IEndpointResult UnprocessableEntity(object? error = null) => UnprocessableEntity<object>(error);


    public static IEndpointResult<T> Ok<T>(object? value = null) => Results.Ok(value).AsEndpointResult<T>();
    public static IEndpointResult<T> Ok<T>(T? value) => Results.Ok(value).AsEndpointResult<T>();
    public static IEndpointResult<T> Accepted<T>(string? uri = null, object? value = null) => Results.Accepted(uri, value).AsEndpointResult<T>();
    public static IEndpointResult<T> Accepted<T>(string? uri = null, T? value = default) => Results.Accepted(uri, value).AsEndpointResult<T>();
    public static IEndpointResult<T> Created<T>(Uri uri, object? value = null) => Results.Created(uri, value).AsEndpointResult<T>();

    public static IEndpointResult<T> CreatedAtRoute<T>(string? routeName = null, object? routeValues = null, object? value = null) =>
        Results.CreatedAtRoute(routeName, routeValues, value).AsEndpointResult<T>();

    public static IEndpointResult<T> CreatedAtRoute<T>(string? routeName = null, object? routeValues = null, T? value = default) =>
        Results.CreatedAtRoute(routeName, routeValues, value).AsEndpointResult<T>();

    public static IEndpointResult<T> Created<T>(string uri, object? value = null) => Results.Created(uri, value).AsEndpointResult<T>();
    public static IEndpointResult<T> NoContent<T>() => Results.NoContent().AsEndpointResult<T>();
    public static IEndpointResult<T> BadRequest<T>(object? error = null) => Results.BadRequest(error).AsEndpointResult<T>();
    public static IEndpointResult<T> BadRequest<T>(T? error) => Results.BadRequest(error).AsEndpointResult<T>();
    public static IEndpointResult<T> Unauthorized<T>() => Results.Unauthorized().AsEndpointResult<T>();

    public static IEndpointResult<T> Forbidden<T>(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null) =>
        Results.Forbid(properties, authenticationSchemes).AsEndpointResult<T>();

    public static IEndpointResult<T> NotFound<T>(T value) => Results.NotFound(value).AsEndpointResult<T>();
    public static IEndpointResult<T> NotFound<T>(object? value = null) => Results.NotFound(value).AsEndpointResult<T>();
    public static IEndpointResult<T> Conflict<T>(object? error = null) => Results.Conflict(error).AsEndpointResult<T>();
    public static IEndpointResult<T> Conflict<T>(T? error) => Results.Conflict(error).AsEndpointResult<T>();
    public static IEndpointResult<T> UnprocessableEntity<T>(object? error = null) => Results.UnprocessableEntity(error).AsEndpointResult<T>();
    public static IEndpointResult<T> UnprocessableEntity<T>(T? error) => Results.UnprocessableEntity(error).AsEndpointResult<T>();
}

public readonly record struct EndpointResult<T>(IResult Result) : IEndpointResult<T>;

public readonly record struct EndpointRequest<TRequest, TResponse>(TRequest Request) : IRequest<IResult>
    where TRequest : IRequest<TResponse>;

internal static class EndpointResultExtensions
{
    public static IEndpointResult<T> AsEndpointResult<T>(this IResult result) => new EndpointResult<T>(result);
}