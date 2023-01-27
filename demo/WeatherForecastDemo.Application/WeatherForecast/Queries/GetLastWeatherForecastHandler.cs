using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Application.Commons;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetLastWeatherForecastQuery : IQuery<Domain.Entities.WeatherForecast>;

public sealed class GetLastWeatherForecastHandler : QueryHandler<GetLastWeatherForecastQuery, Domain.Entities.WeatherForecast>
{
    private readonly IReadOnlyWeatherForecastRepository _forecastRepository;

    public GetLastWeatherForecastHandler(IReadOnlyWeatherForecastRepository forecastRepository)
    {
        _forecastRepository = forecastRepository;
    }

    public override async ValueTask<Result<Domain.Entities.WeatherForecast>> HandleAsync(GetLastWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        return await ValueTask.FromResult(_forecastRepository.GetLast());
    }
}