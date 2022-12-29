using Segres;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public class ConsoleLogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public class PresentationInstaller : IServiceInstaller
{
    public static void Install(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddSingleton<ConsoleLogger>();
    }
}