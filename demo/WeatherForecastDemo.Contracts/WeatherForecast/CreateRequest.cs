using Segres.Abstractions;


namespace WeatherForecastDemo.Contracts.WeatherForecast;

public record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IRequest<bool>, IRequest<Guid>;