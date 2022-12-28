using Segres;
using Segres.AspNet;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpGetRequest("{id:guid}", nameof(WeatherForecast))]
internal record GetWeatherForecastByIdRequest(Guid Id) : IHttpRequest<Domain.Entities.WeatherForecast?>
{
}

internal sealed class GetByIdAbstractEndpoint : IAsyncEndpoint<GetWeatherForecastByIdRequest, Domain.Entities.WeatherForecast?>
{
    private readonly ISender _sender;

    public GetByIdAbstractEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(GetWeatherForecastByIdRequest request, CancellationToken cancellationToken)
    {
        var command = new GetWeatherForecastByIdQuery(request.Id);
        return await _sender.SendAsync(command, cancellationToken);
    }

    public static void Configure(EndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}