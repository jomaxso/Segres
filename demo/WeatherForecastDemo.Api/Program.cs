using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Api.Endpoints.WeatherForecast;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSegres(options => options.WithCustomPublisher<MyPublisher>())
        .AddInstallerRegistrations();

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


    app.MapEndpoints();

    // app.UseSegres();
}

// app.UseCors(x => x.AllowAnyOrigin());
app.Run();

public static class EndpointMapper
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api").WithTags("api");
        
        var v1 = group.MapGroup("v1");
        
        var weatherForecastGroup = v1.MapGroup("WeatherForecasts").WithTags("WeatherForecasts");
        weatherForecastGroup.MapPut<TestRequest, Guid>("test1/{id}");
        weatherForecastGroup.MapGet<TestRequest2, Guid>("test2");
        weatherForecastGroup.MapGet<TestRequest3, Guid>("test3");
    }
}