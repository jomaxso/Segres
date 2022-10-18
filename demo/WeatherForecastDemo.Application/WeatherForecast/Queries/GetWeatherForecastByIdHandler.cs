using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetWeatherForecastByIdQuery(Guid Id) : IQuery<Domain.Entities.WeatherForecast?>;

internal sealed class GetWeatherForecastByIdHandler : IQueryHandler<GetWeatherForecastByIdQuery, Domain.Entities.WeatherForecast?>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetWeatherForecastByIdHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<Domain.Entities.WeatherForecast?> HandleAsync(GetWeatherForecastByIdQuery query, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return await _weatherForecastRepository.GetByIdAsync(query.Id);
    }
}