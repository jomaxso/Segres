using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Application.WeatherForecast.Queries;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public readonly record struct GetAllWeatherForecastsRequest : IHttpRequest<IAsyncEnumerable<WeatherForecast>>
{
    public static string RequestRoute => $"{nameof(WeatherForecast)}/all";
    public static RequestType RequestType => RequestType.Get;
}

public sealed class GetAllEndpoint : AbstractEndpoint<GetAllWeatherForecastsRequest, IAsyncEnumerable<WeatherForecast>>
{
    private readonly IMediator _mediator;

    public GetAllEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<HttpResult<IAsyncEnumerable<WeatherForecast>>> ResolveAsync(GetAllWeatherForecastsRequest request, CancellationToken cancellationToken)
    {
        var weatherForecasts = await _mediator.SendAsync(new GetAllWeatherForecastQuery(), cancellationToken);
        return Ok(weatherForecasts);
    }
}