using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record struct GetAll : IQuery<IEnumerable<Models.WeatherForecast>>;

/// <inheritdoc />
public class GetEndpoint : IQueryHandler<GetAll, IEnumerable<Models.WeatherForecast>>
{
    /// <inheritdoc />
    public async Task<IEnumerable<Models.WeatherForecast>> HandleAsync(GetAll request, CancellationToken cancellationToken)
    {
        var responses = Database.Db;
        return await Task.FromResult(responses);
    }
}