using Segres.AspNetCore;
using Segres.Contracts;
using Segres.Handlers;

namespace WeatherForecastDemo.Api.Endpoints.WeatherForecasts;

public class ExceptionBehavior<TRequest, TResponse> : AbstractEndpointBehavior<TRequest, TResponse> 
    where TResponse : IResult<TResponse, IResult>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public override async ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await next(request, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical("{Message}", e.Message);
            return TResponse.Create(Results.Problem());
        }
    }
}

