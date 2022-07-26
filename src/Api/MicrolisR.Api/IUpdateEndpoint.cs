

using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;


public interface IUpdateEndpoint<in TId, in TEntity>
{
    Task<IActionResult> UpdateAsync([FromRoute] TId id, [FromBody] TEntity entity);
}
