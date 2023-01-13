using Segres.Contracts;
using Segres.Handlers;

namespace WeatherForecastDemo.Api.Endpoints.Notifications;

internal record struct WeatherForecastCreated(Domain.Entities.WeatherForecast WeatherForecast) : INotification;

internal class WeatherForecastCreatedNotificationHandler : INotificationHandler<WeatherForecastCreated>
{
    public ValueTask HandleAsync(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WeatherForecastCreatedNotificationHandler));
        return ValueTask.CompletedTask;
    }
}