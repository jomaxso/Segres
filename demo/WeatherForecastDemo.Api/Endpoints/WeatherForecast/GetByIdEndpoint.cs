using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record GetWeatherForecastByIdRequest(Guid Id) : IHttpRequest
{
}

internal sealed class GetByIdAbstractEndpoint : AbstractEndpoint<GetWeatherForecastByIdRequest>
{
    private readonly ISender _sender;

    public GetByIdAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    protected override async ValueTask<IResult> HandleAsync(GetWeatherForecastByIdRequest request, CancellationToken cancellationToken)
    {
        var command = new GetWeatherForecastByIdQuery(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);

        return Results.Ok(result);
    }

    protected override void Configure(EndpointDefinition builder)
    {
        builder.WithGroup(nameof(WeatherForecast))
            .WithRoute("{id:guid}")
            .MapGet();
    }
}