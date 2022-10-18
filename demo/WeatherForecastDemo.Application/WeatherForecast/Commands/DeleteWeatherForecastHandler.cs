using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record DeleteWeatherForecastCommand(Guid Id) : ICommand<Domain.Entities.WeatherForecast?>;

internal sealed class DeleteWeatherForecastHandler : ICommandHandler<DeleteWeatherForecastCommand, Domain.Entities.WeatherForecast?>
{
    private readonly IWriteOnlyWeatherForecastRepository _writeOnlyWeatherForecastRepository;
    private readonly IReadOnlyWeatherForecastRepository _readOnlyWeatherForecastRepository;


    public DeleteWeatherForecastHandler(IWriteOnlyWeatherForecastRepository writeOnlyWeatherForecastRepository, IReadOnlyWeatherForecastRepository readOnlyWeatherForecastRepository)
    {
        _writeOnlyWeatherForecastRepository = writeOnlyWeatherForecastRepository;
        _readOnlyWeatherForecastRepository = readOnlyWeatherForecastRepository;
    }

    public async Task<Domain.Entities.WeatherForecast?> HandleAsync(DeleteWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
       var weatherForecast = await _readOnlyWeatherForecastRepository.GetByIdAsync(command.Id);

       if (weatherForecast is not null)
           _writeOnlyWeatherForecastRepository.Delete(weatherForecast);
       
       return weatherForecast;
    }
}