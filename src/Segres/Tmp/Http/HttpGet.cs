using System.Diagnostics.CodeAnalysis;

namespace Segres.Tmp.Http;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public sealed class HttpGetAttribute : EndpointAttribute
{
    public HttpGetAttribute(string groupName, [StringSyntax("Route")] string routePattern) 
        : base(groupName, routePattern, Http.GET)
    {
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class HttpPostAttribute : EndpointAttribute
{
    public HttpPostAttribute(string groupName, [StringSyntax("Route")] string routePattern) 
        : base(groupName, routePattern, Http.POST)
    {
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class HttpDeleteAttribute : EndpointAttribute
{
    public HttpDeleteAttribute(string groupName, [StringSyntax("Route")] string routePattern) 
        : base(groupName, routePattern, Http.DELETE)
    {
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class HttpPutAttribute : EndpointAttribute
{
    public HttpPutAttribute(string groupName, [StringSyntax("Route")] string routePattern) 
        : base(groupName, routePattern, Http.PUT)
    {
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public abstract class EndpointAttribute : Attribute
{
    public string GroupName { get; }
    public string? RoutePattern { get; }
    public Http HttpMethod { get; }

    public EndpointAttribute(string groupName, string routePattern, [StringSyntax("Route")] Http httpMethod)
    {
        HttpMethod = httpMethod;
        GroupName = groupName;
        RoutePattern = routePattern;
    }
}