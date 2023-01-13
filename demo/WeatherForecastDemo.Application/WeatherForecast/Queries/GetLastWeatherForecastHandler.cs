using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetLastWeatherForecastQuery : IRequest<Domain.Entities.WeatherForecast>;

public sealed class GetLastWeatherForecastHandler : IRequestHandler<GetLastWeatherForecastQuery, Domain.Entities.WeatherForecast>
{
    private readonly IReadOnlyWeatherForecastRepository _forecastRepository;

    public GetLastWeatherForecastHandler(IReadOnlyWeatherForecastRepository forecastRepository)
    {
        _forecastRepository = forecastRepository;
    }

    public ValueTask<Domain.Entities.WeatherForecast> HandleAsync(GetLastWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_forecastRepository.GetLast());
    }
}