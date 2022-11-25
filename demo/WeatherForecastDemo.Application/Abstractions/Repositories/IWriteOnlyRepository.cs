using System.Diagnostics.CodeAnalysis;

namespace WeatherForecastDemo.Application.Abstractions.Repositories;

public interface IWriteOnlyRepository<TEntity, in TId>
{
    void Delete([NotNull] TEntity entity);
    void DeleteById(TId id);

    void Update([NotNull] TEntity entity);

    TEntity Add([NotNull] TEntity entity);
}