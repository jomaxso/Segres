using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNetCore;

public class HttpPutRequestAttribute : HttpRequestAttribute
{
    public HttpPutRequestAttribute([StringSyntax("Route")] string pattern = "/", string group = "") : base(pattern, group)
    {
    }
}