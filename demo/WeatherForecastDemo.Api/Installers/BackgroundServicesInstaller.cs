using Segres;

namespace WeatherForecastDemo.Api.Installers;

public class BackgroundServicesInstaller : IServiceInstaller
{
    public static void Install(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddHostedService<NotificationWorker>();
    }
}