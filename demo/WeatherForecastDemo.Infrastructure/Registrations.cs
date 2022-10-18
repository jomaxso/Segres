using Microsoft.Extensions.DependencyInjection;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Infrastructure.Repositories;

namespace WeatherForecastDemo.Infrastructure;

public static class Registrations
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IReadOnlyWeatherForecastRepository, WeatherForecastRepository>();
        services.AddSingleton<IWriteOnlyWeatherForecastRepository, WeatherForecastRepository>();
    }
}