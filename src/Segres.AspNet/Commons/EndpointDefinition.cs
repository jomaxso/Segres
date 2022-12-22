using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Segres.AspNet;

public abstract class EndpointDefinition
{
    private readonly MemberInfo _requestType;
    private readonly HttpRequestAttribute? _requestAttribute;
    private bool _isMapCalled;

    private string Group { get; set; }

    protected EndpointDefinition(MemberInfo type)
    {
        _requestType = type;
        _requestAttribute = type.GetCustomAttribute<HttpRequestAttribute>();
        Group = _requestAttribute?.Group ?? string.Empty;
    }

    public EndpointDefinition WithGroup(string group)
    {
        ThrowIfAttributeExists();
        this.Group = group;
        return this;
    }

    public RouteHandlerBuilder MapPost([StringSyntax("Route")] string route = "/")
    {
        ThrowIfAttributeExists();
        _isMapCalled = true;
        return InternalMapPost(route, this.Group)
            .WithTags(Group);
    }

    public RouteHandlerBuilder MapGet([StringSyntax("Route")] string route = "/")
    {
        ThrowIfAttributeExists();
        _isMapCalled = true;
        return InternalMapGet(route, this.Group)
            .WithTags(Group);
    }

    public RouteHandlerBuilder MapPut([StringSyntax("Route")] string route = "/")
    {
        ThrowIfAttributeExists();
        _isMapCalled = true;
        return InternalMapPut(route, this.Group)
            .WithTags(Group);
    }

    public RouteHandlerBuilder MapDelete([StringSyntax("Route")] string route = "/")
    {
        ThrowIfAttributeExists();
        _isMapCalled = true;
        return InternalMapDelete(route, this.Group)
            .WithTags(Group);
    }

    public RouteHandlerBuilder MapFromAttribute()
    {
        var routeHandler = _requestAttribute switch
        {
            HttpGetRequestAttribute http => InternalMapGet(http.Pattern, http.Group),
            HttpPostRequestAttribute http => InternalMapPost(http.Pattern, http.Group),
            HttpPutRequestAttribute http => InternalMapPut(http.Pattern, http.Group),
            HttpDeleteRequestAttribute http => InternalMapDelete(http.Pattern, http.Group),
            null => throw new AmbiguousImplementationException($"Missing configurations for endpoint of type {_requestType}"),
            _ => throw new UnreachableException()
        };

        _isMapCalled = true;
        return routeHandler;
    }

    internal void EnsureMapCalled()
    {
        if (_isMapCalled is false)
        {
            throw new Exception();
        }
    }

    private void ThrowIfAttributeExists()
    {
        if (_requestAttribute is not null)
            throw new AmbiguousImplementationException();
    }

    protected internal abstract RouteHandlerBuilder InternalMapPost([StringSyntax("Route")] string route, string group = "");
    protected internal abstract RouteHandlerBuilder InternalMapGet([StringSyntax("Route")] string route, string group = "");
    protected internal abstract RouteHandlerBuilder InternalMapPut([StringSyntax("Route")] string route, string group = "");
    protected internal abstract RouteHandlerBuilder InternalMapDelete([StringSyntax("Route")] string route, string group = "");
}