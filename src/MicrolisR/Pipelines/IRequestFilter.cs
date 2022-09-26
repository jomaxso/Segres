namespace MicrolisR.Pipelines;

public delegate ValueTask<TResponse> RequestFilterDelegate<in TRequest, TResponse>(TRequest request, CancellationToken cancellationToken);

public interface IRequestFilter<TRequest, TResponse>
{
    ValueTask<TResponse> FilterAsync(RequestFilterDelegate<TRequest, TResponse> next, TRequest request, CancellationToken cancellationToken);
}

public class RequestFilter<TRequest, TResponse> : IRequestFilter<TRequest, TResponse>
{

    public async ValueTask<TResponse> FilterAsync(RequestFilterDelegate<TRequest, TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        var x = await next(request, cancellationToken);
        return x;
    }
}

public class ValidationFilter : IRequestFilter<REQUEST, RESPONSE>
{
    public async ValueTask<RESPONSE> FilterAsync(RequestFilterDelegate<REQUEST, RESPONSE> next, REQUEST request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return null;
        }
        
        var x = await next(request, cancellationToken);

        if (x is null)
        {
            throw new Exception();
        }

        return x;
    }
}




public class REQUEST
{
    
}

public class RESPONSE
{
    
}