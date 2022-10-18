using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal sealed record DeleteRequest(Guid Id) : ICommand<IResult>;

internal sealed class DeleteEndpoint : ICommandHandler<DeleteRequest, IResult>
{
    private readonly ISender _sender;

    public DeleteEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);
        return Results.Ok(result);
    }
}