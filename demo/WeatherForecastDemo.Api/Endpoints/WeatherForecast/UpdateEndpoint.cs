using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record UpdateRequest(Guid Id, Domain.Entities.WeatherForecast WeatherForecast) : ICommand<IResult>;

public sealed class UpdateEndpoint : ICommandHandler<UpdateRequest, IResult>
{
    private readonly ISender _sender;

    public UpdateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(UpdateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(request.Id, request.WeatherForecast);

        var response = await _sender.SendAsync(command, cancellationToken);
        
        return Results.Ok(response);
    }
}