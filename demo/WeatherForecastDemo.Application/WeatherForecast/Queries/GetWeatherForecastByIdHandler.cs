using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetWeatherForecastByIdQuery(Guid Id) : IRequest<Domain.Entities.WeatherForecast?>;

internal sealed class GetWeatherForecastByIdHandler : IRequestHandler<GetWeatherForecastByIdQuery, Domain.Entities.WeatherForecast?>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetWeatherForecastByIdHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(GetWeatherForecastByIdQuery query, CancellationToken cancellationToken = default)
    {
        await ValueTask.CompletedTask;
        return await _weatherForecastRepository.GetByIdAsync(query.Id);
    }
}