using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public record struct CreateWeatherForecastCommand(int TemperatureC, string? Summary) : ICommand<Domain.Entities.WeatherForecast>;

internal class CreateWeatherForecastHandler : ICommandHandler<CreateWeatherForecastCommand, Domain.Entities.WeatherForecast>
{
    private readonly IPublisher _publisher;
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public CreateWeatherForecastHandler(IPublisher publisher, IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _publisher = publisher;
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<Domain.Entities.WeatherForecast> HandleAsync(CreateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        var entity = new Domain.Entities.WeatherForecast()
        {
            Date = DateTime.Now,
            TemperatureC = command.TemperatureC,
            Summary = command.Summary
        };

        var result = _weatherForecastRepository.Add(entity);

        var message = new WeatherForecastCreated(entity);
        
        await _publisher.PublishAsync(message, cancellationToken);

        return result;
    }
}

internal record struct WeatherForecastCreated(Domain.Entities.WeatherForecast WeatherForecast) : IMessage;