using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record struct GetAllRequest : IQuery<IResult>;

internal sealed class GetAllEndpoint : IQueryHandler<GetAllRequest, IResult>
{
    private readonly ISender _sender;

    public GetAllEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllWeatherForecastQuery();
        var result = await _sender.SendAsync(query, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}