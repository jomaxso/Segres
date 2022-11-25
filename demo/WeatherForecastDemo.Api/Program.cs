using FluentValidation;
using Segres;
using WeatherForecastDemo.Api.Endpoints;
using WeatherForecastDemo.Api.Endpoints.WeatherForecast;
using WeatherForecastDemo.Application;
using WeatherForecastDemo.Application.Commons.Behaviors;
using WeatherForecastDemo.Contracts.WeatherForecast;
using WeatherForecastDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddInfrastructure();

    builder.Services.AddSegres(x =>
    {
        x.RegisterAssembly(typeof(IApplicationMarker).Assembly);
        x.RegisterAssembly(typeof(CreateWeatherForecastRequest).Assembly);
        x.WithEndpoints();
    });

    // builder.Services.AddScoped(typeof(IRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
    builder.Services.AddScoped(typeof(IRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
    builder.Services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(includeInternalTypes: true);

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

    // app.UseSegres(x => { x.Configure<GetAllRequest>(); });
    app.MapSegres<GetRangedRequestTest>();
    
    // app.MapGroupedEndpoints("tests",group =>
    // {
    //     const string route = "{from:int}/{till:int}";
    //     group.MapGetEndpoint<GetRangedRequestTest, IEnumerable<int>>(route);
    // });
}

app.Run();