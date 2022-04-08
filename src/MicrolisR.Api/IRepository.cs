namespace MicrolisR.Data.Abstraction;

public interface IRepository<TId, TEntity>
    where TEntity : IEntity<TId>
    where TId : struct
{
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetAsync(TId id);
    Task DeleteAsync(TId id);
    Task UpdateAsync(TId id, TEntity entity);
    Task CreateAsync(TEntity entity);
}