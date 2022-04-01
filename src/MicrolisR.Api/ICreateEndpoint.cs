using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]
public interface ICreateEndpoint<in TEntity>
{
    [HttpPost]
    Task<IActionResult> CreateAsync([FromBody] TEntity entity);
}
