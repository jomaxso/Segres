using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public readonly record struct DeleteRequest(Guid Id) : IHttpRequest<WeatherForecast>
{
    public static string RequestRoute => "WeatherForecast/{id:guid}";
    public static RequestType RequestType => RequestType.Delete;
}

public sealed class DeleteAbstractRequestEndpoint : AbstractEndpoint<DeleteRequest, WeatherForecast>
{
    private readonly IMediator _mediator;

    public DeleteAbstractRequestEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<HttpResult<WeatherForecast>> ResolveAsync(DeleteRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteWeatherForecastCommand
        {
            Id = request.Id
        };
        
        var result = await _mediator.SendAsync(command, cancellationToken);
        return result is null ? NotFound<WeatherForecast>("Item not found") : Ok(result);
    }
}