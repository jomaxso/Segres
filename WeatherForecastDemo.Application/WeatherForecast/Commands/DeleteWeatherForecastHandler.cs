using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record DeleteWeatherForecastCommand(Domain.Entities.WeatherForecast WeatherForecast) : ICommand<Domain.Entities.WeatherForecast>;

internal sealed class DeleteWeatherForecastHandler : ICommandHandler<DeleteWeatherForecastCommand, Domain.Entities.WeatherForecast>
{
    private IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public DeleteWeatherForecastHandler(IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<Domain.Entities.WeatherForecast> HandleAsync(DeleteWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
         _weatherForecastRepository.Delete(command.WeatherForecast);

         return command.WeatherForecast;
    }
}