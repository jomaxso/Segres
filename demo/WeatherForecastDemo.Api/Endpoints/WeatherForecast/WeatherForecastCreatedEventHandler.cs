using Segres;
using Segres.Abstractions;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record struct WeatherForecastCreatedEvent(Guid Id) : INotification;

public class WeatherForecastCreatedEventHandler : IAsyncNotificationHandler<WeatherForecastCreatedEvent>
{
    public ValueTask HandleAsync(WeatherForecastCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(notification));
        return ValueTask.CompletedTask;
    }
}