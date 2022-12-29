using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNetCore;

public class HttpDeleteRequestAttribute : HttpRequestAttribute
{
    public HttpDeleteRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}