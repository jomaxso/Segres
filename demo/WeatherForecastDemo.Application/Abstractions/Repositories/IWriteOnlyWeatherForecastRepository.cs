namespace WeatherForecastDemo.Application.Abstractions.Repositories;

public interface IWriteOnlyWeatherForecastRepository : IWriteOnlyRepository<Domain.Entities.WeatherForecast, Guid>
{

}