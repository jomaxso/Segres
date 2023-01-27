using Segres;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Application.Commons;

public abstract class QueryHandler<TRequest, TResult> : IRequestHandler<TRequest, Result<TResult>> 
    where TRequest : IQuery<TResult>
{
    public abstract ValueTask<Result<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}