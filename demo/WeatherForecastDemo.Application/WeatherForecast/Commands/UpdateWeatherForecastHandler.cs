using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Application.Commons;

namespace WeatherForecastDemo.Application.WeatherForecast.Commands;

public sealed record UpdateWeatherForecastCommand(Guid Id, Domain.Entities.WeatherForecast WeatherForecast) : ICommand<Domain.Entities.WeatherForecast>;

internal sealed class UpdateWeatherForecastHandler : IRequestHandler<UpdateWeatherForecastCommand, Result<Domain.Entities.WeatherForecast>>
{
    private readonly IWriteOnlyWeatherForecastRepository _weatherForecastRepository;

    public UpdateWeatherForecastHandler(IWriteOnlyWeatherForecastRepository weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async ValueTask<Result<Domain.Entities.WeatherForecast>> HandleAsync(UpdateWeatherForecastCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        _weatherForecastRepository.Update(command.WeatherForecast);

        return command.WeatherForecast;
    }
}