using Segres;
using Segres.Abstractions;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

public record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IRequest<Guid>;

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
        builder.MapPost();
    }

    public async ValueTask<IEndpointResult<Guid>> ResolveAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        await _sender.SendAsync(command, cancellationToken);
        var id = Guid.NewGuid();
        await _publisher.PublishAsync(new WeatherForecastCreatedEvent(id), cancellationToken);
        
        return EndpointResult.Ok(id);
    }
}

public class CreateWeatherForecastRequestBehavior : IRequestBehavior<CreateWeatherForecastCommand, Guid>
{
    public Guid Handle(RequestHandlerDelegate<Guid> next, CreateWeatherForecastCommand request)
    {
        return next(request);
    }
}