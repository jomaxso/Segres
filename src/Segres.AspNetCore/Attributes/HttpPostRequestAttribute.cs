using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNetCore;

public class HttpPostRequestAttribute : HttpRequestAttribute
{
    public HttpPostRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}