using System.Linq.Expressions;
using WeatherForecastDemo.Application.Abstractions.Repositories;
using WeatherForecastDemo.Domain.Entities;

namespace WeatherForecastDemo.Infrastructure.Repositories;

public class WeatherForecastRepository :
    IReadOnlyWeatherForecastRepository,
    IWriteOnlyWeatherForecastRepository
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly List<WeatherForecast> _cache = Enumerable.Range(1, 500).Select(index => new WeatherForecast
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();

    public void DeleteById(Guid id)
    {
        var toDelete = _cache.First(x => x.Id == id);
        Delete(toDelete);
    }

    public void Update(WeatherForecast? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _cache.RemoveAll(x => x.Id == entity.Id);
        _cache.Add(entity);
    }

    public WeatherForecast Add(WeatherForecast entity)
    {
        entity.Id = Guid.NewGuid();
        _cache.Add(entity);

        return entity;
    }


    public void Delete(WeatherForecast entity)
    {
        _cache.Remove(entity);
    }


    public async Task<List<WeatherForecast>> GetAsync(
        Expression<Func<WeatherForecast, bool>>? filter = null,
        Func<IQueryable<WeatherForecast>, IOrderedQueryable<WeatherForecast>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default,
        bool trackable = true)
    {
        await Task.CompletedTask;
        var query = _cache.AsQueryable();

        if (filter is not null)
            query = query.Where(filter);

        if (orderBy is not null)
            query = orderBy(query);

        if (string.IsNullOrWhiteSpace(includeProperties) is false)
        {
        }

        if (trackable)
        {
        }

        return query.ToList();
    }

    public async ValueTask<WeatherForecast?> GetByIdAsync(Guid id)
    {
        await ValueTask.CompletedTask;
        return _cache.FirstOrDefault(x => x.Id == id);
    }

    public async IAsyncEnumerable<WeatherForecast> Get(Expression<Func<WeatherForecast, bool>>? filter = null, Func<IQueryable<WeatherForecast>, IOrderedQueryable<WeatherForecast>>? orderBy = null,
        string includeProperties = "", bool trackable = true)
    {
        var query = _cache.AsQueryable();

        if (filter is not null)
            query = query.Where(filter);

        if (orderBy is not null)
            query = orderBy(query);

        if (string.IsNullOrWhiteSpace(includeProperties) is false)
        {
        }

        if (trackable)
        {
        }

        foreach (var weatherForecast in query.ToList())
        {
            await ValueTask.CompletedTask;
            yield return weatherForecast;
        }
    }
}