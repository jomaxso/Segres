namespace Segres.AspNet;

public abstract class AbstractEndpoint<TRequest> : IAsyncRequestHandler<TRequest, IHttpResult>, IEndpointConfiguration
    where TRequest : IHttpRequest
{
    protected virtual void Configure(EndpointDefinition builder)
        => builder.MapFromAttribute();

    void IEndpointConfiguration.Configure(EndpointDefinition builder)
    {
        Configure(builder);
        builder.EnsureMapCalled();
    }

    public abstract ValueTask<IHttpResult> HandleAsync(TRequest request, CancellationToken cancellationToken);

    protected IHttpResult Ok() => new OkHttpResult();
    protected IHttpResult BadRequest(Error? error = null) => new BadRequestHttpResult(error);
}

public abstract class AbstractEndpoint<TRequest, TResponse> : IAsyncRequestHandler<TRequest, IHttpResult<TResponse>>, IEndpointConfiguration
    where TRequest : IHttpRequest<TResponse>
{
    protected virtual void Configure(EndpointDefinition builder)
        => builder.MapFromAttribute();

    void IEndpointConfiguration.Configure(EndpointDefinition builder)
    {
        Configure(builder);
        builder.EnsureMapCalled();
    }

    public abstract ValueTask<IHttpResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);

    protected IHttpResult<TResponse> Ok(TResponse response) => new OkHttpResult<TResponse>(response);
    protected IHttpResult<TResponse> BadRequest(Error? error = null) => new BadRequestHttpResult<TResponse>(error);
}