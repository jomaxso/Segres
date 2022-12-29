using FluentValidation;
using Segres;
using Segres.Abstractions;
using WeatherForecastDemo.Application;
using WeatherForecastDemo.Application.Commons.Behaviors;

namespace WeatherForecastDemo.Api.Installers;

public class ValidationInstaller : IServiceInstaller
{
    public static void Install(IServiceCollection services, IConfiguration? configuration)
    {
        // builder.Services.AddScoped(typeof(IAsyncRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
        services.AddScoped(typeof(IAsyncRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(includeInternalTypes: true);

    }
}