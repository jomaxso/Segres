using System.ComponentModel.DataAnnotations;
using MicrolisR;
using MicrolisR.Pipelines;
using WebApplication1.Endpoints.WeatherForecast;

namespace WebApplication1.Endpoints;

/// <inheritdoc />
public class ValidationBehavior : IPipelineBehavior<WeatherForecastGetById, Models.WeatherForecast>
{
    /// <inheritdoc />
    public async Task<IQuery<Models.WeatherForecast>> BeforeAsync(WeatherForecastGetById request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return request;
    }

    /// <inheritdoc />
    public async Task<Models.WeatherForecast> AfterAsync(Models.WeatherForecast response, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return response;
    }
}