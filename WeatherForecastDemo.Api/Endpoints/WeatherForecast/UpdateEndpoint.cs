using Segres;
using Segres.Endpoint;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record UpdateRequest(Guid Id, Domain.Entities.WeatherForecast WeatherForecast) : IPutRequest;

public sealed class UpdateEndpoint : IPutEndpoint<UpdateRequest>
{
    private readonly ISender _sender;

    public UpdateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> ExecuteAsync(UpdateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(request.Id, request.WeatherForecast);

        var response = await _sender.SendAsync(command, cancellationToken);
        
        return Results.Ok(response);
    }
}