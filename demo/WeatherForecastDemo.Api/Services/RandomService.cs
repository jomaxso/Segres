namespace WeatherForecastDemo.Api.Services;

public class RandomService
{
    private readonly Random _random = Random.Shared;

    public int GetNext(int min, int max) => _random.Next(min, max + 1);
}