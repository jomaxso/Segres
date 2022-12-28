using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpGetRequest(group: nameof(WeatherForecast))]
internal record GetAllRequest(int? Number) : IHttpRequest<IEnumerable<Domain.Entities.WeatherForecast>>
{
}

internal sealed class GetAllAbstractEndpoint : IAsyncEndpoint<GetAllRequest, IEnumerable<Domain.Entities.WeatherForecast>>
{
    private readonly ISender _sender;

    public GetAllAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<IEnumerable<Domain.Entities.WeatherForecast>> HandleAsync(GetAllRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var query = new GetAllWeatherForecastQuery(request.Number);
        var response = _sender.Send(query);
        return response;
    }

    public static void Configure(EndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}