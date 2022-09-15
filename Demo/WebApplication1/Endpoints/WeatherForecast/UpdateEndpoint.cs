using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record UpdateWeatherForecastRequest(DateTime Date, int TemperatureC, string? Summary) : IQueryRequest<Models.WeatherForecast>
{
    internal int Id { get; set; }
}

public class UpdateEndpoint : IQueryRequestHandler<UpdateWeatherForecastRequest, Models.WeatherForecast>
{
    private readonly IPublisher _publisher;
    
    public UpdateEndpoint(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    /// <inheritdoc />
    public async Task<Models.WeatherForecast> HandleAsync(UpdateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var weatherForecast = Database.Db.First(x => x.Id == request.Id);
        weatherForecast.Date = request.Date;
        weatherForecast.Summary = request.Summary;
        weatherForecast.TemperatureC = request.TemperatureC;

         _publisher.PublishAsync(new WeatherForecastChanged(weatherForecast), cancellationToken);
        
        return await Task.FromResult(weatherForecast);
    }
}

public record UpdateWeatherForecastRequest2(DateTime Date, int TemperatureC, string? Summary) : IQueryRequest<Models.WeatherForecast>
{
    internal int Id { get; set; }
}

public class UpdateEndpoint2 : IQueryRequestHandler<UpdateWeatherForecastRequest2, Models.WeatherForecast>
{
    private readonly IPublisher _publisher;
    
    public UpdateEndpoint2(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    /// <inheritdoc />
    public async Task<Models.WeatherForecast> HandleAsync(UpdateWeatherForecastRequest2 request, CancellationToken cancellationToken)
    {
        var weatherForecast = Database.Db2.First(x => x.Id == request.Id);
        weatherForecast.Date = request.Date;
        weatherForecast.Summary = request.Summary;
        weatherForecast.TemperatureC = request.TemperatureC;

         _publisher.PublishAsync(new WeatherForecastChanged2(weatherForecast), cancellationToken);
        
        return await Task.FromResult(weatherForecast);
    }
}