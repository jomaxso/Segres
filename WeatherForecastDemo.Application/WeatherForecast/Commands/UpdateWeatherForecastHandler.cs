using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record UpdateWeatherForecastCommand(Guid Id, Domain.Entities.WeatherForecast WeatherForecast) : ICommand<Domain.Entities.WeatherForecast>;

internal sealed class UpdateWeatherForecastHandler : ICommandHandler<UpdateWeatherForecastCommand, Domain.Entities.WeatherForecast>
{
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public UpdateWeatherForecastHandler(IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<Domain.Entities.WeatherForecast> HandleAsync(UpdateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        _weatherForecastRepository.Update(command.WeatherForecast);

        return command.WeatherForecast;
    }
}