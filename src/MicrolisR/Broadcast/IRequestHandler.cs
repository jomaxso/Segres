namespace MicrolisR;

/// <summary>
/// Defines a receiver for a request. (just for internal usage) 
/// </summary>
/// <seealso cref="IQueryRequestHandler{TRequest}"/>
/// <seealso cref="ICommandRequestHandler{TRequest,TResponse}"/>
public interface IInternalRequestHandler
{
    
    internal Task HandleAsync<T>(T request, CancellationToken cancellationToken);
}