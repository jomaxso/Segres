using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNet;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
public abstract class HttpRequestAttribute : Attribute
{
    public string Pattern { get; }
    public string Group { get; }

    public HttpRequestAttribute([StringSyntax("Route")] string pattern, string group)
    {
        Pattern = pattern;
        Group = group;
    }
}

public class HttpGetRequestAttribute : HttpRequestAttribute
{
    public HttpGetRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}

public class HttpPostRequestAttribute : HttpRequestAttribute
{
    public HttpPostRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}

public class HttpPutRequestAttribute : HttpRequestAttribute
{
    public HttpPutRequestAttribute([StringSyntax("Route")] string pattern = "/", string group = "") : base(pattern, group)
    {
    }
}

public class HttpDeleteRequestAttribute : HttpRequestAttribute
{
    public HttpDeleteRequestAttribute([StringSyntax("Route")] string pattern= "/", string group = "") : base(pattern, group)
    {
    }
}