using Segres.Abstractions;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpDeleteRequest("{id:guid}", nameof(WeatherForecast))]
public record DeleteRequest(Guid Id) : IRequest<Domain.Entities.WeatherForecast?>;

public sealed class DeleteAbstractRequestEndpoint : IAsyncRequestEndpoint<DeleteRequest, Domain.Entities.WeatherForecast>
{
    private readonly ISender _sender;

    public DeleteAbstractRequestEndpoint(ISender sender)
    {
        _sender = sender;
    }
    

    public static void Configure(IEndpointDefinition builder)
    {
        builder.MapFromAttribute()
            .WithName("DeleteWeatherForecast");
    }

    public async ValueTask<IEndpointResult<Domain.Entities.WeatherForecast?>> ResolveAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);
        return EndpointResult.Ok(result);
    }
}