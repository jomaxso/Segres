using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetAllWeatherForecastQuery(int? Number = null) : IStreamRequest<Domain.Entities.WeatherForecast>;

internal sealed class GetAllWeatherForecastHandler : IStreamRequestHandler<GetAllWeatherForecastQuery, Domain.Entities.WeatherForecast>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetAllWeatherForecastHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public IAsyncEnumerable<Domain.Entities.WeatherForecast> HandleAsync(GetAllWeatherForecastQuery request, CancellationToken cancellationToken)
        => _weatherForecastRepository.Get();
}