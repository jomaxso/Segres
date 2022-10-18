using System.Linq.Expressions;

namespace WeatherForecastDemo.Application.Abstractions.Repositories;

public interface IReadOnlyRepository<TEntity, in TId>
{
    Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default,
        bool trackable = true);

    ValueTask<TEntity?> GetByIdAsync(TId id);
}