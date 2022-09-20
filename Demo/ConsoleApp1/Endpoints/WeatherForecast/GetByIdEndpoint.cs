using MicrolisR;

namespace WebApplication1.Endpoints.WeatherForecast;

/// <summary>
/// 
/// </summary>
public class WeatherForecastGetByIdRequest : IQueryRequest<Models.WeatherForecast?>
{
    public int Id { get; init; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static implicit operator WeatherForecastGetByIdRequest(int id)
    {
        return new WeatherForecastGetByIdRequest()
        {
            Id = id
        };
    }
}

/// <summary>
/// 
/// </summary>
public record struct WeatherForecastGetAllRequest : IQueryRequest<List<Models.WeatherForecast>>
{
}

/// <summary>
/// 
/// </summary>
public class GetByIdEndpoint :
    IQueryRequestHandler<WeatherForecastGetByIdRequest, Models.WeatherForecast?>,
    IQueryRequestHandler<WeatherForecastGetAllRequest, List<Models.WeatherForecast>>
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
    public async Task<Models.WeatherForecast?> HandleAsync(WeatherForecastGetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }

    /// <inheritdoc />
    public async Task<List<Models.WeatherForecast>> HandleAsync(WeatherForecastGetAllRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}