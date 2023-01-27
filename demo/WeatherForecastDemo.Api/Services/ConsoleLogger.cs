

namespace WeatherForecastDemo.Api.Services;

public class ConsoleLogger : IConsoleLogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public interface IConsoleLogger
{
    public void Log(string message);
}