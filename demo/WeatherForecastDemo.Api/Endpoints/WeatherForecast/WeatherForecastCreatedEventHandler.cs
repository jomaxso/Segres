using Segres;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public class WeatherForecastCreatedEventHandler : INotificationHandler<WeatherForecastCreatedEvent>
{
    public ValueTask HandleAsync(WeatherForecastCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(notification));
        return ValueTask.CompletedTask;
    }
}