using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Commands;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public sealed record CreateWeatherForecastRequest(int TemperatureC, string? Summary) : IHttpRequest
{
    public static string RequestRoute => $"{nameof(WeatherForecast)}/";
    public static RequestType RequestType => RequestType.Post;
}

public sealed class CreateAbstractRequestEndpoint : AbstractEndpoint<CreateWeatherForecastRequest>
{
    private readonly IMediator _mediator;
    
    public CreateAbstractRequestEndpoint(IMediator mediator) =>  _mediator = mediator;

    public override void Configure(RouteHandlerBuilder routeBuilder)
    {
        routeBuilder
            .Accepts<CreateWeatherForecastRequest>("application/json")
            .Produces(200)
            .ProducesProblem(400)
            .ProducesProblem(500);
    }

    public override async ValueTask<HttpResult> ResolveAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeatherForecastCommand
        {
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };
        var id = await _mediator.SendAsync(command, cancellationToken);
        return HttpResult.Create(Results.Ok());
    }
}