using Segres;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Contracts.WeatherForecast;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal sealed class CreateEndpoint : IEndpoint<CreateWeatherForecastRequest, None>
{
    private readonly ISender _sender;

    public CreateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<None> ExecuteAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        await _sender.SendAsync(command, cancellationToken);

        // return result is null
        //     ? Results.BadRequest()
        //     : Results.Ok(result);

        return None.Empty;
    }
}