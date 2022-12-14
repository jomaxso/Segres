using Microsoft.AspNetCore.Http;

namespace Segres.AspNet;

public abstract class AbstractEndpoint<TRequest> : IRequestHandler<EndpointRequest<TRequest>, IResult>, IEndpointConfiguration
    where TRequest : IHttpRequest
{

    protected virtual void Configure(EndpointDefinition builder)
    {
    }
    
    protected abstract ValueTask<IResult> HandleAsync(TRequest request, CancellationToken cancellationToken);

    void IEndpointConfiguration.Configure(EndpointDefinition builder)
    {
        // TODO hier können Attribute Eingelesen werden
        
        
        Configure(builder);
    }
    
    ValueTask<IResult> IRequestHandler<EndpointRequest<TRequest>, IResult>.HandleAsync(EndpointRequest<TRequest> request, CancellationToken cancellationToken)
    {
        return HandleAsync(request.Request, cancellationToken);
    }
}