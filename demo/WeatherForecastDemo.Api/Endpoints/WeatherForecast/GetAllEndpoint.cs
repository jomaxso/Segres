using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;


internal record GetAllRequest(int? Number) : IHttpRequest
{
}

internal sealed class GetAllAbstractEndpoint : AbstractEndpoint<GetAllRequest>
{
    private readonly ISender _sender;

    public GetAllAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }


    protected override async ValueTask<IResult> HandleAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllWeatherForecastQuery(request.Number);
        var result = await _sender.SendAsync(query, cancellationToken);

        return Results.Ok(result);
    }

    protected override void Configure(EndpointDefinition definition)
    {
        definition.WithGroup(nameof(WeatherForecast))
            .WithRoute("/")
            .MapGet();
    }
}