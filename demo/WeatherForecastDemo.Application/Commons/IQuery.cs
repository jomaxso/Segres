using Segres;

namespace WeatherForecastDemo.Application.Commons;

public interface IQuery<T> : IRequest<Result<T>>
{
    
}