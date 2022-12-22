using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

public record GetFirstFiveWeatherForecastRequest : IRequest<IEnumerable<Domain.Entities.WeatherForecast>>;

public class GetFirstFiveWeatherForecastHandler : IAsyncRequestHandler<GetFirstFiveWeatherForecastRequest, IEnumerable<Domain.Entities.WeatherForecast>>
{
    private readonly IReadOnlyWeatherForecastRepository _repository;

    public GetFirstFiveWeatherForecastHandler(IReadOnlyWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<Domain.Entities.WeatherForecast>> HandleAsync(GetFirstFiveWeatherForecastRequest request, CancellationToken cancellationToken = default)
    {
        return (await _repository.GetAsync(cancellationToken: cancellationToken)).Take(5);
    }
}