using Segres;

namespace WeatherForecastDemo.Api.Endpoints.Notifications;

internal record struct WeatherForecastCreated(Domain.Entities.WeatherForecast WeatherForecast) : INotification;

internal class WeatherForecastCreatedErrorLogNotificationHandler : INotificationHandler<WeatherForecastCreated>
{
    public async ValueTask HandleAsync(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine(nameof(WeatherForecastCreatedErrorLogNotificationHandler));
    }
}

internal class WeatherForecastCreatedInformationLogNotificationHandler : INotificationHandler<WeatherForecastCreated>
{
    public async ValueTask HandleAsync(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine(nameof(WeatherForecastCreatedInformationLogNotificationHandler));
    }
}