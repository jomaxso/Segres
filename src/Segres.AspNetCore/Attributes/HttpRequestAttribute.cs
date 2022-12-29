using System.Diagnostics.CodeAnalysis;

namespace Segres.AspNetCore;

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