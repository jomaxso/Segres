using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]

public interface IDeleteEndpoint<TId>
{
    [HttpDelete("{id}")]
    Task<IActionResult> DeleteAsync([FromRoute] TId id);
}
