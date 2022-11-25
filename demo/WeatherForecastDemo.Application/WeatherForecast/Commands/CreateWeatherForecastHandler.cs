using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public record struct CreateWeatherForecastCommand(int TemperatureC, string? Summary) : IRequest;

internal class CreateWeatherForecastHandler : IRequestHandler<CreateWeatherForecastCommand>
{
    private readonly IPublisher _publisher;
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public CreateWeatherForecastHandler(IPublisher publisher, IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _publisher = publisher;
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async ValueTask HandleAsync(CreateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        var entity = new Domain.Entities.WeatherForecast
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            TemperatureC = command.TemperatureC,
            Summary = command.Summary
        };

        var result = _weatherForecastRepository.Add(entity);

        var message = new WeatherForecastCreated(entity);

        await _publisher.PublishAsync(message, cancellationToken);
    }
}

internal record struct WeatherForecastCreated(Domain.Entities.WeatherForecast WeatherForecast) : INotification;