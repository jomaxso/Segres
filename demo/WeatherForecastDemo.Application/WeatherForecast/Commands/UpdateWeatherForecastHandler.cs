using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record UpdateWeatherForecastCommand(Guid Id, Domain.Entities.WeatherForecast WeatherForecast) : IRequest<Domain.Entities.WeatherForecast>;

internal sealed class UpdateWeatherForecastHandler : IRequestHandler<UpdateWeatherForecastCommand, Domain.Entities.WeatherForecast>
{
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public UpdateWeatherForecastHandler(IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async ValueTask<Domain.Entities.WeatherForecast> HandleAsync(UpdateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        _weatherForecastRepository.Update(command.WeatherForecast);

        return command.WeatherForecast;
    }
}