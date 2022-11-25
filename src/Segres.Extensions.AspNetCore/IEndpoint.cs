using Segres;
using Segres.Tmp.Http;

namespace WeatherForecastDemo.Api.Endpoints;

public interface IEndpoint<in TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
    public ValueTask<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}