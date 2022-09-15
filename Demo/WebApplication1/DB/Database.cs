using WebApplication1.Models;

namespace WebApplication1.DB;

public static class Database
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    internal static List<Models.WeatherForecast> Db = Enumerable.Range(1, 5).Select(index => new Models.WeatherForecast
        {
            Id = index,
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();


    internal static List<Models.WeatherForecast> Db2 = new();
}