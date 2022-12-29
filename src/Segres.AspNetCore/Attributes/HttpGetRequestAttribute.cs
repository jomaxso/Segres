using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNetCore;

public class HttpGetRequestAttribute : HttpRequestAttribute
{
    public HttpGetRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}