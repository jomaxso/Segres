using Segres.AspNet;

namespace WeatherForecastDemo.Contracts.WeatherForecast;

[HttpPostRequest]
public record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IHttpRequest, IHttpRequest<Guid>;