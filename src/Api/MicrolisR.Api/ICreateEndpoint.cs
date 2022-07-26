using MicrolisR.Api.Enumeration;
using Microsoft.AspNetCore.Mvc;

namespace MicrolisR.Api;

[Endpoint("[endpoint]", RequestKind.HttpPost)]
public interface IPostEndpoint<in TRequest>
{
    Task<IActionResult> CreateAsync([FromBody] TRequest request);
}
