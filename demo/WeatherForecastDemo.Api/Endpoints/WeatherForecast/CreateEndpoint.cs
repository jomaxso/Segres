using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Contracts.WeatherForecast;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public sealed class CreateAbstractEndpoint : AbstractEndpoint<CreateWeatherForecastRequest, Guid>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public CreateAbstractEndpoint(ISender sender, IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }

    public override async ValueTask<IHttpResult<Guid>> HandleAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        await _sender.SendAsync(command, cancellationToken);
        var id = Guid.NewGuid();
        await _publisher.PublishAsync(new WeatherForecastCreatedEvent(id), cancellationToken);
        
        return Ok(id);
    }

    protected override void Configure(EndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}