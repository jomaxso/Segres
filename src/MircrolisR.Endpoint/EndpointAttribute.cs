using Microsoft.AspNetCore.Http;

namespace MicrolisR;

[AttributeUsage(AttributeTargets.Method)]
public class EndpointAttribute : Attribute
{
    public EndpointAttribute(Http verb,string route)
    {
        
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ValidateAttribute : Attribute
{
}

