using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record CreateRequest(DateTime Date, int TemperatureC, string? Summary) : ICommand;

/// <inheritdoc />
public class CreateEndpoint : ICommandHandler<CreateRequest>
{
    private readonly IPublisher _publisher;

    public CreateEndpoint(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task HandleAsync(CreateRequest request, CancellationToken cancellationToken)
    {
        var weatherForecast = Map(request);

        Database.Db.Add(weatherForecast);

        _publisher.PublishAsync(new WeatherForecastChanged(weatherForecast), cancellationToken);

        return Task.CompletedTask;
    }

    private static Models.WeatherForecast Map(CreateRequest request)
    {
        return new Models.WeatherForecast()
        {
            Id = Database.Db.Count + 1,
            Date = request.Date,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC
        };
    }
}