using Segres;
using Segres.Abstractions;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpGetRequest(group: nameof(WeatherForecast))]
internal record GetAllRequest() : IStreamRequest<Domain.Entities.WeatherForecast>
{
}

internal sealed class GetAllAbstractEndpoint : IStreamEndpoint<GetAllRequest, Domain.Entities.WeatherForecast>
{
    private readonly ISender _sender;

    public GetAllAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public static void Configure(IEndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }

    public IAsyncEnumerable<Domain.Entities.WeatherForecast> HandleAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllWeatherForecastQuery(1);
        return _sender.Send(query, cancellationToken);
    }
}