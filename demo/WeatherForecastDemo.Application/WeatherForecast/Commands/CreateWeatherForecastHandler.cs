using Segres;
using WeatherForecastDemo.Api.Endpoints.Notifications;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Application.Commons;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public record struct CreateWeatherForecastCommand(int TemperatureC, string? Summary) : ICommand<Guid>;

internal class CreateWeatherForecastHandler : IRequestHandler<CreateWeatherForecastCommand, Result<Guid>>
{
    private readonly IPublisher _publisher;
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public CreateWeatherForecastHandler(IPublisher publisher, IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _publisher = publisher;
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async ValueTask<Result<Guid>> HandleAsync(CreateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        var entity = new Domain.Entities.WeatherForecast
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            TemperatureC = command.TemperatureC,
            Summary = command.Summary
        };

        Console.WriteLine(entity.Date);

        var result = _weatherForecastRepository.Add(entity);
        
        await _publisher.PublishAsync( new WeatherForecastCreated(result), cancellationToken);

        return result.Id;
    }
}

