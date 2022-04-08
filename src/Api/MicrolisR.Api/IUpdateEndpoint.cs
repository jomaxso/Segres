using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]

public interface IUpdateEndpoint<in TId, in TEntity>
{
    [HttpPut("{id}")]
    Task<IActionResult> UpdateAsync([FromRoute] TId id, [FromBody] TEntity entity);
}
