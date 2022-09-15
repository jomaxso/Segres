using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record CreateWeatherForecastRequest(DateTime Date, int TemperatureC, string? Summary) : ICommandRequest;

/// <inheritdoc />
public class CreateEndpoint : ICommandRequestHandler<CreateWeatherForecastRequest>
{
    private readonly IPublisher _publisher;
    
    public CreateEndpoint(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public async Task HandleAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var weatherForecast = new Models.WeatherForecast()
        {
            Id = Database.Db.Count + 1,
            Date = request.Date,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC
        };
        
        Database.Db.Add(weatherForecast);
        
         _publisher.PublishAsync(new WeatherForecastChanged(weatherForecast), cancellationToken);
    }
}