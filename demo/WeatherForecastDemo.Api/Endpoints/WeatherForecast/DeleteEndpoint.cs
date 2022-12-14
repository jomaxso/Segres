using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record DeleteRequest(Guid Id) : IHttpRequest
{
}

public sealed class DeleteAbstractEndpoint : AbstractEndpoint<DeleteRequest>
{
    private readonly ISender _sender;

    public DeleteAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }
    
    protected override void Configure(EndpointDefinition builder)
    {
        builder.WithGroup(nameof(WeatherForecast))
            .WithRoute("{id:guid}")
            .MapDelete();
    }

    protected override async ValueTask<IResult> HandleAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);
        return result is not null ? Results.Ok(result) : Results.BadRequest(Error.Null);
    }
}