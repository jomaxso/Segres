using Segres;
using Segres.Extensions.DependencyInjection.Microsoft;
using WeatherForecastDemo.Api.Endpoints.WeatherForecast;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Domain.Entities;
using WeatherForecastDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddInfrastructure();
    builder.Services.AddSegres(typeof(Program), typeof(CreateWeatherForecastCommand));
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

    app.MapGet("p/{id:guid}", (ISender sender, Guid id, CancellationToken cancellationToken)
        => sender.SendAsync(new GetByIdRequest(id), cancellationToken));

    app.MapGet("p", (ISender sender, CancellationToken cancellationToken)
        => sender.SendAsync(new GetAllRequest(), cancellationToken));

    app.MapPost("p", (ISender sender, CreateRequest request, CancellationToken cancellationToken)
        => sender.SendAsync(request, cancellationToken));

    app.MapPut("p/{id:guid}", (ISender sender, Guid id, WeatherForecast weatherForecast, CancellationToken cancellationToken)
        => sender.SendAsync(new UpdateRequest(id, weatherForecast), cancellationToken));

    app.MapDelete("p/{id}", (ISender sender, Guid id, CancellationToken cancellationToken)
        => sender.SendAsync(new DeleteRequest(id), cancellationToken));
}

app.Run();