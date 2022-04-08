using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Route("[endpoint]")]

public interface IDeleteEndpoint<in TId>
{
    [HttpDelete("{id}")]
    Task<IActionResult> DeleteAsync([FromRoute] TId id);
}
