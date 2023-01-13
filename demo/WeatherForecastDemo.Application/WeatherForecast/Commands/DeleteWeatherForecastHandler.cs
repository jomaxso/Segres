using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.Abstractions.Repositories;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record DeleteWeatherForecastCommand : IRequest<Domain.Entities.WeatherForecast?>
{
    public required Guid Id { get; init; }
}

internal sealed class DeleteWeatherForecastHandler : IRequestHandler<DeleteWeatherForecastCommand, Domain.Entities.WeatherForecast?>
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