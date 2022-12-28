using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpDeleteRequest("{id:guid}", nameof(WeatherForecast))]
public record DeleteRequest(Guid Id) : IHttpRequest<Domain.Entities.WeatherForecast?>;

public sealed class DeleteAbstractEndpoint : IAsyncEndpoint<DeleteRequest, Domain.Entities.WeatherForecast?>
{
    private readonly ISender _sender;

    public DeleteAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);
        return result;
    }

    public static void Configure(EndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}