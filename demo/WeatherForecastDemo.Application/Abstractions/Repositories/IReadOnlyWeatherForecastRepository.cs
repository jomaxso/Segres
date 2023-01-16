namespace WeatherForecastDemo.Application.Abstractions.Repositories;

public interface IReadOnlyWeatherForecastRepository : IReadOnlyRepository<Domain.Entities.WeatherForecast, Guid>
{
    Domain.Entities.WeatherForecast GetLast();
}