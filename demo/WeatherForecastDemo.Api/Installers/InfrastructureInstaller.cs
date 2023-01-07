using Segres;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Infrastructure.Repositories;

namespace WeatherForecastDemo.Api.Installers;

public class InfrastructureInstaller : IServiceInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IReadOnlyWeatherForecastRepository, WeatherForecastRepository>();
        services.AddSingleton<IWriteOnlyWeatherForecastRepository, WeatherForecastRepository>();
    }
}