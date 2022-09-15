using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record struct WeatherForecastGetRequest : IQueryRequest<IEnumerable<Models.WeatherForecast>>;

/// <inheritdoc />
public class GetEndpoint : IQueryRequestHandler<WeatherForecastGetRequest, IEnumerable<Models.WeatherForecast>>
{
    /// <inheritdoc />
    public async Task<IEnumerable<Models.WeatherForecast>> HandleAsync(WeatherForecastGetRequest request, CancellationToken cancellationToken)
    {
        var responses = Database.Db;
        return await Task.FromResult(responses);
    }
}