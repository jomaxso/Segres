using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record struct WeatherForecastGetByIdRequest(int Id) : IQueryRequest<Models.WeatherForecast?>;

/// <inheritdoc />
public class GetByIdEndpoint : IQueryRequestHandler<WeatherForecastGetByIdRequest, Models.WeatherForecast?>
{
    /// <inheritdoc />
    public async Task<Models.WeatherForecast?> HandleAsync(WeatherForecastGetByIdRequest request, CancellationToken cancellationToken)
    {
        var weatherForecast = Database.Db.FirstOrDefault(x => x.Id == request.Id);
        return await Task.FromResult(weatherForecast);
    }
}