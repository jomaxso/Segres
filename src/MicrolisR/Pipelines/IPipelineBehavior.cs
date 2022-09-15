namespace MicrolisR.Pipelines;

/// <summary>
/// 
/// </summary>
public interface IPipelineBehavior
{
    internal Task BeforeAsync<TRequest>(TRequest request, CancellationToken cancellationToken);

    internal Task AfterAsync<TResponse>(TResponse response, CancellationToken cancellationToken);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IPipelineBehavior<in TRequest, TResponse> : IPipelineBehavior
    where TRequest : IQueryRequest<TResponse>
{
    Task IPipelineBehavior.BeforeAsync<T>(T request, CancellationToken cancellationToken)
        where T : default
    {
        if (request is TRequest r)
            return BeforeAsync(r, cancellationToken);
        
        throw new Exception();
    }

    Task IPipelineBehavior.AfterAsync<T>(T response, CancellationToken cancellationToken)
        where T : default
    {
        if (response is null)
            return AfterAsync(default!, cancellationToken);

        if (response is TResponse r)
            return AfterAsync(r, cancellationToken);

        throw new Exception();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IQueryRequest<TResponse>> BeforeAsync(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> AfterAsync(TResponse response, CancellationToken cancellationToken);
}