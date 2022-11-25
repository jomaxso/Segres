using Microsoft.AspNetCore.Builder;

namespace WeatherForecastDemo.Api.Endpoints;

public record EndpointOptions
{
    private static IDictionary<object, EndpointOptions> _cache = new Dictionary<object, EndpointOptions>();

    public RouteHandlerBuilder Configure<T>()
    {
        return default!;
    }  
}