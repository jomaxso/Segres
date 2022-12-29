using Segres;
using Segres.Abstractions;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record DeleteWeatherForecastCommand(Guid Id) : IRequest<Domain.Entities.WeatherForecast?>;

internal sealed class DeleteWeatherForecastHandler : IAsyncRequestHandler<DeleteWeatherForecastCommand, Domain.Entities.WeatherForecast?>
{
    private readonly IReadOnlyWeatherForecastRepository _readOnlyWeatherForecastRepository;
    private readonly IWriteOnlyWeatherForecastRepository _writeOnlyWeatherForecastRepository;


    public DeleteWeatherForecastHandler(IWriteOnlyWeatherForecastRepository writeOnlyWeatherForecastRepository, IReadOnlyWeatherForecastRepository readOnlyWeatherForecastRepository)
    {
        _writeOnlyWeatherForecastRepository = writeOnlyWeatherForecastRepository;
        _readOnlyWeatherForecastRepository = readOnlyWeatherForecastRepository;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(DeleteWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var weatherForecast = await _readOnlyWeatherForecastRepository.GetByIdAsync(command.Id);

        if (weatherForecast is not null)
            _writeOnlyWeatherForecastRepository.Delete(weatherForecast);

        return weatherForecast;
    }
}