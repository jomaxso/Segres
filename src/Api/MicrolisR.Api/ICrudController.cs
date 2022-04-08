using MicrolisR.Data.Abstraction;

namespace MicrolisR.Api;

[Route("[endpoint]")]
public interface ICrudController<TId, TEntity> :
    ICreateEndpoint<TEntity>,
    IReadEndpoint<TEntity>,
    IReadEndpoint<TId, TEntity>,
    IUpdateEndpoint<TId, TEntity>,
    IDeleteEndpoint<TId>
    where TEntity : class, IEntity<TId>
    where TId : struct
{ }