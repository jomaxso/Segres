using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]
public interface IGetEndpoint<TEntity>
{
    [HttpGet]
    Task<ActionResult<IEnumerable<TEntity>>> GetAsync();
}

[Route("[endpoint]")]
public interface IGetEndpoint<in TId, TEntity>
{
    [HttpGet("{id}")]
    Task<ActionResult<TEntity?>> GetAsync([FromRoute] TId id);
}
