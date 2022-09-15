using System.ComponentModel.DataAnnotations;
using MicrolisR;
using MicrolisR.Pipelines;
using WebApplication1.DB;
using WebApplication1.Endpoints.WeatherForecast;

namespace WebApplication1.Endpoints;

public record struct WeatherForecastChanged(Models.WeatherForecast Value) : INotification;

public class WeatherForecastChangedEvent : INotificationHandler<WeatherForecastChanged>
{
    public Task HandleAsync(WeatherForecastChanged notification, CancellationToken cancellationToken)
    {
        Database.Db2.Add(Database.Db.Last(x => x.Id == notification.Value.Id));
        return Task.CompletedTask;
    }
}

public record struct WeatherForecastChanged2(Models.WeatherForecast Value) : INotification;

public class WeatherForecastChangedEvent2 : INotificationHandler<WeatherForecastChanged2>
{
    public Task HandleAsync(WeatherForecastChanged2 notification, CancellationToken cancellationToken)
    {
        Database.Db.Add(Database.Db2.Last(x => x.Id == notification.Value.Id));
        return Task.CompletedTask;
    }
}

public class ValidationBehavior : IPipelineBehavior<WeatherForecastGetByIdRequest, Models.WeatherForecast>
{
    public async Task<IQueryRequest<Models.WeatherForecast>> BeforeAsync(WeatherForecastGetByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        if (request.Id < 0)
        {
            throw new ValidationException("id must be in positiv range");
        }

        return request;
    }

    public async Task<Models.WeatherForecast?> AfterAsync(Models.WeatherForecast? response, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        if (response is null)
        {
            throw new NotImplementedException();
        }

        return response;
    }
}