using MicrolisR;
using WebApplication1.DB;

namespace WebApplication1.Endpoints.WeatherForecast;

public record struct GetById(int Id) 
    : IQuery<Models.WeatherForecast?>;

/// <inheritdoc />
public class GetByIdEndpoint : IQueryHandler<GetById, Models.WeatherForecast?>
{
    /// <inheritdoc />
    public async Task<Models.WeatherForecast?> HandleAsync(GetById request, CancellationToken cancellationToken)
    {
        var weatherForecast = Database.Db.FirstOrDefault(x => x.Id == request.Id);
        return await Task.FromResult(weatherForecast);
    }
}