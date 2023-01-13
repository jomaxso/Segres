using FluentValidation;
using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Api.Endpoints.WeatherForecasts;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    var config = builder.Services
        .AddSegres();
        // .AddSegres(options =>
        // {
        //     options.UseReferencedAssemblies(typeof(Program));
        //     options.UsePublisherContext<MyPublisher>();
        //     options.UseParallelNotification();
        //     options.UseLifetime(ServiceLifetime.Singleton);
        // });
    
    builder.Services.AddSingleton<IReadOnlyWeatherForecastRepository, WeatherForecastRepository>();
    builder.Services.AddSingleton<IWriteOnlyWeatherForecastRepository, WeatherForecastRepository>();
    builder.Services.AddHostedService<NotificationWorker>();
    builder.Services.AddSingleton<IConsoleLogger, ConsoleLogger>();
    
    builder.Services.AddValidatorsFromAssemblies(config.Assemblies, ServiceLifetime.Singleton, includeInternalTypes: true);

    builder.Services.AddAuthorization();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.UseSegres();

}

// app.UseCors(x => x.AllowAnyOrigin());
app.Run();
