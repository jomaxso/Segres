using Segres;

namespace WeatherForecastDemo.Application.Commons;

public interface ICommand<T> : IRequest<Result<T>>
{
    
}