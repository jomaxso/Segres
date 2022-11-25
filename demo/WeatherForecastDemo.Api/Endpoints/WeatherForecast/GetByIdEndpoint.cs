using Microsoft.AspNetCore.Mvc;
using Segres;
using WeatherForecastDemo.Api.Endpoints.Abstractions;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[Segres.Tmp.Http.HttpGet("WeatherForecast", "/id")]
internal record GetByIdRequest([FromQuery] Guid Id) : IHttpRequest
{
}

internal sealed class GetByIdEndpoint : IEndpoint<GetByIdRequest>
{
    private readonly ISender _sender;


    public GetByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<IResult> ExecuteAsync(GetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var command = new GetWeatherForecastByIdQuery(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}