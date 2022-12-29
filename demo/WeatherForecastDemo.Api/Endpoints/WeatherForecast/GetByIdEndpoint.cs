using Segres;
using Segres.Abstractions;
using Segres;
using Segres.AspNetCore;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

[HttpGetRequest("{id:guid}", nameof(WeatherForecast))]
internal record GetWeatherForecastByIdRequest(Guid Id) : IRequest<Domain.Entities.WeatherForecast?>
{
}

internal sealed class GetByIdAbstractRequestEndpoint : IAsyncRequestEndpoint<GetWeatherForecastByIdRequest, Domain.Entities.WeatherForecast?>
{
    private readonly ISender _sender;

    public GetByIdAbstractRequestEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async ValueTask<Domain.Entities.WeatherForecast?> HandleAsync(GetWeatherForecastByIdRequest request, CancellationToken cancellationToken)
    {
        var command = new GetWeatherForecastByIdQuery(request.Id);
        return await _sender.SendAsync(command, cancellationToken);
    }

    public static void Configure(IEndpointDefinition builder)
    {
        builder.MapFromAttribute();
    }
}