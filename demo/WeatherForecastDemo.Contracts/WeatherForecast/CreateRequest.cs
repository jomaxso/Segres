using Segres;
using Segres.Tmp.Http;


namespace WeatherForecastDemo.Contracts.WeatherForecast;

[HttpPost("WeatherForecast", "")]
public record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IHttpRequest<None>
{
}

[HttpPut("HUHU", "")]
public record TestRequest : IHttpRequest<bool>
{
    public string Name { get; init; }
}

