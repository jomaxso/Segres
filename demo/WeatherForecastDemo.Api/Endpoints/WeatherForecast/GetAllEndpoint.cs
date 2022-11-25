using Segres;
using Segres.Tmp.Http;
using WeatherForecastDemo.Api.Endpoints.Abstractions;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpGet("WeatherForecast", "")]
internal record GetAllRequest(int? Number) : IHttpRequest
{
}

internal sealed class GetAllEndpoint : IEndpoint<GetAllRequest>
{
    private readonly ISender _sender;

    public GetAllEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<IResult> ExecuteAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllWeatherForecastQuery(request.Number);
        var result = await _sender.SendAsync(query, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}