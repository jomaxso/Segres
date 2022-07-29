using System.Reflection;
using MicrolisR.Internal;

namespace MicrolisR;

public interface IHttpContextRequestResolver<in TRequest, TResponse> : IHttpContextRequestResolver
    where TRequest : IRequestable<TResponse>
{
    IRequestHandler<TRequest, TResponse> RequestHandler { get; }
    
    MethodInfo IHttpContextRequestResolver.MethodInfo
    {
        get
        {
            var requestMethod = typeof(IRequestHandler<TRequest, TResponse>)
                .GetMethod(nameof(IRequestHandler<TRequest, TResponse>.HandleAsync))!;
            
            var method = RequestHandler
                .GetType()
                .GetMethods()
                .First(x => x.Name == requestMethod.Name && x.GetParameters()[0].ParameterType == typeof(TRequest));
    
            return method;
        }
    }
}

public interface IHttpContextRequestResolver
{
    public MethodInfo MethodInfo { get; }
    public Delegate EndpointDelegate { get; }
    
    public sealed Http HttpVerb => GetHttpVerb();
    public sealed string Route => GetHttpRoute();
    public sealed bool Validate => ShouldValidate();
    
    private Http GetHttpVerb()
    {
        var endpointAttribute = MethodInfo.GetCustomAttribute<EndpointAttribute>();
        Throw.IfNull(endpointAttribute);
        return endpointAttribute.Verb;
    }
    
    private string GetHttpRoute()
    {
        var endpointAttribute = MethodInfo.GetCustomAttribute<EndpointAttribute>();
        Throw.IfNull(endpointAttribute);
        return endpointAttribute.Route;
    }
    
    private bool ShouldValidate()
    {
        var endpointAttribute = MethodInfo.GetCustomAttribute<EndpointAttribute>();
        Throw.IfNull(endpointAttribute);
        return endpointAttribute.Validate;
    }
}