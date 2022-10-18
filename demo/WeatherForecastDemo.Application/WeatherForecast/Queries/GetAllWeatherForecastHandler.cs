using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetAllWeatherForecastQuery() : IQuery<IEnumerable<Domain.Entities.WeatherForecast>>;

internal sealed class GetAllWeatherForecastHandler : IQueryHandler<GetAllWeatherForecastQuery, IEnumerable<Domain.Entities.WeatherForecast>>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetAllWeatherForecastHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<IEnumerable<Domain.Entities.WeatherForecast>> HandleAsync(GetAllWeatherForecastQuery query, CancellationToken cancellationToken = default)
    {
        return await _weatherForecastRepository.GetAsync(cancellationToken: cancellationToken);
    }
}