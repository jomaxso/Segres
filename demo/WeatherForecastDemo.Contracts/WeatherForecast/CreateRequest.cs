using Microsoft.AspNetCore.Mvc;
using Segres;
using Segres.AspNet;



namespace WeatherForecastDemo.Contracts.WeatherForecast;

// [HttpPost("WeatherForecast", "")]
public record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IHttpRequest
{
}
