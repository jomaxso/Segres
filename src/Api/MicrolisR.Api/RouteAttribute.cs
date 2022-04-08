namespace MicrolisR.Api;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class RouteAttribute : Microsoft.AspNetCore.Mvc.RouteAttribute
{
    public RouteAttribute(string template) : base(template)
    {
    }
}
