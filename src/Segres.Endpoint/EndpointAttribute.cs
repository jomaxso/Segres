namespace Segres.Endpoint;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class EndpointRouteAttribute : Attribute
{
    public string Route { get; }

    public EndpointRouteAttribute(string route)
    {
        this.Route = route;
    }
}