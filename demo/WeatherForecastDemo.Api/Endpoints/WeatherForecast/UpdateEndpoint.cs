using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpPutRequest("{id:guid}", nameof(WeatherForecast))]
public record UpdateWeatherForecastRequest(Guid Id, DateTime Date, int TemperatureC, string? Summary) : IHttpRequest<Domain.Entities.WeatherForecast?>;

public sealed class UpdateAbstractEndpoint : AbstractEndpoint<UpdateWeatherForecastRequest, Domain.Entities.WeatherForecast?>
{
    private readonly ISender _sender;

    public UpdateAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override async ValueTask<IHttpResult<Domain.Entities.WeatherForecast?>> HandleAsync(UpdateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(request.Id, new Domain.Entities.WeatherForecast
        {
            Date = request.Date,
            Id = request.Id,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC
        });
        
        var result = await _sender.SendAsync(command, cancellationToken);
        
        return Ok(result);
    }
}