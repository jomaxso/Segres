using Segres;
using Segres.Tmp.Http;
using WeatherForecastDemo.Api.Endpoints.Abstractions;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpPut("WeatherForecast", "{id:guid}")]
public record UpdateRequest(Guid Id, DateTime Date, int TemperatureC, string? Summary) : IHttpRequest
{
    public static string RoutePattern => "{id:guid}";
    public static Http HttpMethod => Http.PUT;
}

public sealed class UpdateEndpoint : IEndpoint<UpdateRequest>
{
    private readonly ISender _sender;

    public UpdateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<IResult> ExecuteAsync(UpdateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(request.Id, new Domain.Entities.WeatherForecast
        {
            Date = request.Date,
            Id = request.Id,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC
        });

        var response = await _sender.SendAsync(command, cancellationToken);

        return Results.Ok(response);
    }
}