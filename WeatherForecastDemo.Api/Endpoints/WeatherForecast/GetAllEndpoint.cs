using Segres;
using Segres.Endpoint;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record struct GetAllRequest : IGetRequest;

internal sealed class GetAllEndpoint : IGetEndpoint<GetAllRequest>
{
    private readonly ISender _sender;

    public GetAllEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [EndpointRoute("WeatherForecast")]
    public async Task<IResult> ExecuteAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllWeatherForecastQuery();
        var result = await _sender.SendAsync(query, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}