using Segres;
using Segres.Endpoint;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal sealed record DeleteRequest(Domain.Entities.WeatherForecast WeatherForecast) : IDeleteRequest;

internal sealed class DeleteEndpoint : IDeleteEndpoint<DeleteRequest>
{
    private readonly ISender _sender;

    public DeleteEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> ExecuteAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.WeatherForecast);
        var result = await _sender.SendAsync(command, cancellationToken);
        return Results.Ok(result);
    }
}