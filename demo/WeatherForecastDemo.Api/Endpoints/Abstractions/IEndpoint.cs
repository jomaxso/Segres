namespace WeatherForecastDemo.Api.Endpoints.Abstractions;

public interface IEndpoint<in TRequest> : IEndpoint<TRequest, IResult>
    where TRequest : IHttpRequest
{
}