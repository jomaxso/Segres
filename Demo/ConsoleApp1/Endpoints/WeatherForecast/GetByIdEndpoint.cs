using MicrolisR;

namespace WebApplication1.Endpoints.WeatherForecast;

/// <summary>
/// 
/// </summary>
public class WeatherForecastGetByIdRequest : IQueryRequest<Models.WeatherForecast>
{
    /// <summary></summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class GetByIdEndpoint : IQueryRequestHandler<WeatherForecastGetByIdRequest, Models.WeatherForecast>
{
    
    /// <inheritdoc />
    public async Task<Models.WeatherForecast> HandleAsync(WeatherForecastGetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}


public record WeatherForecastGetByIdRequest2(int Id) : IQueryRequest<Models.WeatherForecast>;

/// <inheritdoc />
public class GetByIdEndpoint2 : IQueryRequestHandler<WeatherForecastGetByIdRequest2, Models.WeatherForecast>
{
    
    /// <inheritdoc />
    public async Task<Models.WeatherForecast> HandleAsync(WeatherForecastGetByIdRequest2 request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}