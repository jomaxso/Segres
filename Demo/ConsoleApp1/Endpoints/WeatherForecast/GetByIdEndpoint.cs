using MediatR;
using MicrolisR;

namespace WebApplication1.Endpoints.WeatherForecast;

/// <summary>
/// 
/// </summary>
public class WeatherForecastGetById : IQuery<Models.WeatherForecast?>, IRequest<Models.WeatherForecast?>
{
    public int Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static implicit operator WeatherForecastGetById(int id)
    {
        return new WeatherForecastGetById()
        {
            Id = id
        };
    }
}

/// <summary>
/// 
/// </summary>
public class GetByIdEndpoint :
    IQueryHandler<WeatherForecastGetById, Models.WeatherForecast?>,
    IRequestHandler<WeatherForecastGetById, Models.WeatherForecast?>
{
    private static List<Models.WeatherForecast> db = new()
    {
        new()
        {
            Date = DateTime.Now.AddDays(-1),
            Id = 1,
            Summary = "Cloudy",
            TemperatureC = 17
        },
        new()
        {
            Date = DateTime.Now,
            Id = 2,
            Summary = "Cold",
            TemperatureC = 3
        },
        new()
        {
            Date = DateTime.Now.AddDays(1),
            Id = 3,
            Summary = "Frezzing",
            TemperatureC = -6
        },
    };

    /// <inheritdoc />
    public async Task<Models.WeatherForecast?> HandleAsync(WeatherForecastGetById request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }

    /// <inheritdoc />
    public async Task<Models.WeatherForecast?> Handle(WeatherForecastGetById request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}