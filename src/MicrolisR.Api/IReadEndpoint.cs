using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]
public interface IReadEndpoint<TEntity>
{
    [HttpGet]
    Task<ActionResult<IEnumerable<TEntity>>> GetAsync();
}

[Route("[endpoint]")]
public interface IReadEndpoint<TId, TEntity>
{
    [HttpGet("{id}")]
    Task<ActionResult<TEntity?>> GetAsync([FromRoute] TId id);
}
