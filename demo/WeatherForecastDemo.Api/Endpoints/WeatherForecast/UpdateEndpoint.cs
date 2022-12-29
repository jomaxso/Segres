using Segres;
using Segres.Abstractions;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpPutRequest("{id:guid}", nameof(WeatherForecast))]
public record UpdateWeatherForecastRequest(Guid Id, DateTime Date, int TemperatureC, string? Summary) : IRequest<Domain.Entities.WeatherForecast?>;

public sealed class UpdateAbstractRequestEndpoint : IAsyncRequestEndpoint<UpdateWeatherForecastRequest, Domain.Entities.WeatherForecast?>
{
    private readonly ISender _sender;

    public UpdateAbstractRequestEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(UpdateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(request.Id, new Domain.Entities.WeatherForecast
        {
            Date = request.Date,
            Id = request.Id,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC
        });
        
        var result = await _sender.SendAsync(command, cancellationToken);
        
        return result;
    }

    public static void Configure(IEndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}