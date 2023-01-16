using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Queries;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public sealed record GetLastWeatherForecastRequest : IHttpRequest<WeatherForecast>
{
    public static string RequestRoute => "/WeatherForecast/last";
    public static RequestType RequestType => RequestType.Get;
}

public class GetLastEndpoint : AbstractEndpoint<GetLastWeatherForecastRequest, WeatherForecast>
{
    private readonly IMediator _mediator;

    public GetLastEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<HttpResult<WeatherForecast>> ResolveAsync(GetLastWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetLastWeatherForecastQuery(), cancellationToken);
        return result;
    }
}