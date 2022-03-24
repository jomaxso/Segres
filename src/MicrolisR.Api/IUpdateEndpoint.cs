using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]

public interface IUpdateEndpoint<TId, TEntity>
{
    [HttpPut("{id}")]
    Task<IActionResult> UpdateAsync([FromRoute] TId id, [FromBody] TEntity entity);
}
