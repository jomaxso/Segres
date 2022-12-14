using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Contracts.WeatherForecast;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record struct WeatherForecastCreatedEvent(Guid Id) : INotification;

public sealed class CreateAbstractEndpoint : AbstractEndpoint<CreateWeatherForecastRequest>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public CreateAbstractEndpoint(ISender sender, IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }

    protected override void Configure(EndpointDefinition builder)
    {
        builder
            .WithGroup(nameof(WeatherForecast))
            .WithRoute("/")
            .MapPost();
    }

    protected override async ValueTask<IResult> HandleAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        var id = await _sender.SendAsync(command, cancellationToken);
        await _publisher.PublishAsync(new WeatherForecastCreatedEvent(id), cancellationToken);
        return Results.Ok(id);
    }
}