using Segres;
using Segres.Contracts;
using Segres.Endpoint;
using Segres.Handlers;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record struct CreateRequest(int TemperatureC, string? Summary) : IPostRequest;

internal sealed class CreateEndpoint : IPostEndpoint<CreateRequest>
{
    private readonly ISender _sender;

    public CreateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [EndpointRoute("WeatherForecast")]
    public async Task<IResult> ExecuteAsync(CreateRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand()
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        var result = await _sender.SendAsync(command, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}