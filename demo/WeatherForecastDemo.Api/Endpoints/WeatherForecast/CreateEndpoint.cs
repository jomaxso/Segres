using Segres.Abstractions;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Contracts.WeatherForecast;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public sealed class CreateAbstractRequestEndpoint : IAsyncRequestEndpoint<CreateWeatherForecastRequest, Guid>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public CreateAbstractRequestEndpoint(ISender sender, IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }
    
    public static void Configure(IEndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
    
    public async ValueTask<Guid> HandleAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        await _sender.SendAsync(command, cancellationToken);
        var id = Guid.NewGuid();
        await _publisher.PublishAsync(new WeatherForecastCreatedEvent(id), cancellationToken);

        return id;
    }
}