using Microsoft.AspNetCore.Mvc;
using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record UpdateWeatherForecastRequest(Guid Id, DateTime Date, int TemperatureC, string? Summary) : IHttpRequest;

public sealed class UpdateAbstractEndpoint : AbstractEndpoint<UpdateWeatherForecastRequest>
{
    private readonly ISender _sender;

    public UpdateAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    protected override async ValueTask<IResult> HandleAsync(UpdateWeatherForecastRequest request, CancellationToken cancellationToken)
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

    protected override void Configure(EndpointDefinition builder)
    {
        builder.WithGroup(nameof(WeatherForecast))
            .WithRoute("{id:guid}")
            .MapPut();
    }
}