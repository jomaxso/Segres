using Segres.AspNetCore;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public sealed record UpdateWeatherForecastRequest : IHttpRequest<WeatherForecast>
{
    public static string RequestRoute => $"{nameof(WeatherForecast)}";
    public static RequestType RequestType => RequestType.Put;

    public WeatherForecast Forecast { get; init; } = default!;
}

public sealed class UpdateEndpoint : AbstractEndpoint<UpdateWeatherForecastRequest, WeatherForecast>
{
    private readonly IWriteOnlyWeatherForecastRepository _writeOnlyWeatherForecastRepository;

    public UpdateEndpoint(IWriteOnlyWeatherForecastRepository writeOnlyWeatherForecastRepository)
    {
        _writeOnlyWeatherForecastRepository = writeOnlyWeatherForecastRepository;
    }

    public override async ValueTask<HttpResult<WeatherForecast>> ResolveAsync(UpdateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        _writeOnlyWeatherForecastRepository.Update(request.Forecast);
        await ValueTask.CompletedTask;

        return request.Forecast;
    }
}