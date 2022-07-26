using MicrolisR.Api.Enumeration;

namespace MicrolisR.Api;

[AttributeUsage(AttributeTargets.Class)]
public class EndpointAttribute : Attribute
{
    public EndpointAttribute(string route, RequestKind kind, BindContext bindFrom = BindContext.Default, bool allowAnonymous = true)
    {
        Route = route;
        Kind = kind;
        Binding = bindFrom;
        AllowAnonymous = allowAnonymous;
    }

    public string Route { get; }
    public RequestKind Kind { get; }
    public BindContext Binding { get; }
    public bool AllowAnonymous { get; }
}


//[AttributeUsage(AttributeTargets.Class)]
//public class ApiEndpointAttribute<TRequest, TResponse> : ApiEndpointAttribute
//{
//    public ApiEndpointAttribute(string route, RequestKind requestKind, BindContext bindingContext = BindContext.Default)
//        : base(route, requestKind, bindingContext)
//    {
//    }
//}





