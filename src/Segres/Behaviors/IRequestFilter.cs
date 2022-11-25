namespace Segres;

public interface IRequestFilter<TRequest>
{
    public ValueTask<TRequest> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequestFilterAttribute<TFilter, T> : Attribute where TFilter : IRequestFilter<T>
{
    
}

