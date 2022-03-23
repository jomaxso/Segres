using MicrolisR.Data.Abstraction;

namespace MicrolisR.Api;

public class CrudController<TId, TEntity>
    where TEntity : IEntity<TId>
{
    private readonly IRepository<TId, TEntity> _repository;

    protected CrudController(IRepository<TId, TEntity> repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<TEntity>> GetAsync() => _repository.GetAsync();
    public Task<TEntity?> GetAsync(TId id) => _repository.GetAsync(id);
    public Task DeleteAsync(TId id) => _repository.DeleteAsync(id);
    public Task UpdateAsync(TId id, TEntity? entity) => _repository.UpdateAsync(id, entity);
    public Task CreateAsync(TEntity? entity) => _repository.CreateAsync(entity);
}

//[AttributeUsage(AttributeTargets.Class)]
//public class CrudControllerAttribute<TRepository, TId, TEntity> : RouteAttribute
//    where TRepository : IRepository<TId, TEntity>
//{
//    public CrudControllerAttribute(string groupeName = "[controller]") : base(groupeName)
//    {

//    }
//}

