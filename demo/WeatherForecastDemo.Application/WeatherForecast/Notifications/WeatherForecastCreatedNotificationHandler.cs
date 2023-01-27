using Segres;

namespace WeatherForecastDemo.Api.Endpoints.Notifications;

internal record struct WeatherForecastCreated(Domain.Entities.WeatherForecast WeatherForecast) : IEvent;

internal class WeatherForecastCreatedErrorLogEventHandler : IEventHandler<WeatherForecastCreated>
{
    public async ValueTask HandleAsync(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine(nameof(WeatherForecastCreatedErrorLogEventHandler));
    }
}

internal class WeatherForecastCreatedInformationLogEventHandler : IEventHandler<WeatherForecastCreated>
{
    public async ValueTask HandleAsync(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine(nameof(WeatherForecastCreatedInformationLogEventHandler));
    }
}