using Segres;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public class ConsoleLogger : IConsoleLogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public interface IConsoleLogger
{
    public void Log(string message);
}

public class PresentationInstaller : IServiceInstaller
{
    public static void Install(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddSingleton<IConsoleLogger, ConsoleLogger>();
    }
}