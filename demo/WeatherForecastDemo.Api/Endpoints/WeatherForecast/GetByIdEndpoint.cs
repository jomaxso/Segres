using Segres;
using Segres.Contracts;
using Segres.Handlers;
using WeatherForecastDemo.Application.WeatherForecast.Queries;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecast;

internal record struct GetByIdRequest(Guid Id) : IQuery<IResult>;

internal sealed class GetByIdEndpoint : IQueryHandler<GetByIdRequest, IResult>
{
    private readonly ISender _sender;


    public GetByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task<IResult> HandleAsync(GetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var command = new GetWeatherForecastByIdQuery(request.Id);
        var result = await _sender.SendAsync(command, cancellationToken);

        return result is null
            ? Results.BadRequest()
            : Results.Ok(result);
    }
}

