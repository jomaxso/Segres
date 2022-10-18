using Segres;
using Segres.Endpoint;
using Segres.Extensions.DependencyInjection.Microsoft;
using WeatherForecastDemo.Api.Endpoints.WeatherForecast;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
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

    app.MapPost<CreateRequest>("p");
    app.MapPut<UpdateRequest>("p/{id}");
    app.MapDelete<DeleteRequest>("p");
}

app.Run();



