using Segres;
using Segres.Tmp.Http;
using WeatherForecastDemo.Api.Endpoints.Abstractions;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpDelete("WeatherForecast", "{id}")]
internal record DeleteRequest(Guid Id) : IHttpRequest
{
}

internal sealed class DeleteEndpoint : IEndpoint<DeleteRequest>
{
    private readonly ISender _sender;

    public DeleteEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<IResult> ExecuteAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);
        return Results.Ok(result);
    }
}