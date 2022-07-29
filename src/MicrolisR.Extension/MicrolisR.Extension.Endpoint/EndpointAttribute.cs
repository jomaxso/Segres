namespace MicrolisR;

[AttributeUsage(AttributeTargets.Method)]
public class EndpointAttribute : Attribute
{
    public Http Verb { get; }
    public string Route { get; }
    public bool Validate { get; }

    public EndpointAttribute(Http verb, string route, bool validate = true)
    {
        Verb = verb;
        Route = route;
        Validate = validate;
    }
}