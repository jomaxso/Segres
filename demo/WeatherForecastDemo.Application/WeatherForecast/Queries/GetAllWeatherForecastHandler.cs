using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetAllWeatherForecastQuery(int? Number = null) : IRequest<IEnumerable<Domain.Entities.WeatherForecast>>;

internal sealed class GetAllWeatherForecastHandler : IRequestHandler<GetAllWeatherForecastQuery, IEnumerable<Domain.Entities.WeatherForecast>>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetAllWeatherForecastHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public IEnumerable<Domain.Entities.WeatherForecast> Handle(GetAllWeatherForecastQuery request)
    {
        return _weatherForecastRepository.Get();
    }
}