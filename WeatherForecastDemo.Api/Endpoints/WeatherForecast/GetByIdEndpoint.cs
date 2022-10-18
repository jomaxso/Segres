using Segres;
using Segres.Endpoint;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record struct GetByIdRequest(Guid Id) : IGetRequest;

internal sealed class GetByIdEndpoint : IGetEndpoint<GetByIdRequest>
{
    private readonly ISender _sender;


    public GetByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [EndpointRoute("WeatherForecast/{id}")]
    public async Task<IResult> ExecuteAsync(GetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var command = new GetWeatherForecastByIdQuery(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}

