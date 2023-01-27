using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Application.Commons;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public sealed record GetAllWeatherForecastQuery(int? Number = null) : IQuery<IEnumerable<Domain.Entities.WeatherForecast>>;

internal sealed class GetAllWeatherForecastHandler : QueryHandler<GetAllWeatherForecastQuery, IEnumerable<Domain.Entities.WeatherForecast>>
{
    private readonly IReadOnlyWeatherForecastRepository _weatherForecastRepository;

    public GetAllWeatherForecastHandler(IReadOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public override async ValueTask<Result<IEnumerable<Domain.Entities.WeatherForecast>>> HandleAsync(GetAllWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var enumerable = _weatherForecastRepository.Get()
            .ToBlockingEnumerable(cancellationToken: cancellationToken)
            .AsResult();

        return await ValueTask.FromResult(enumerable);
    }
}