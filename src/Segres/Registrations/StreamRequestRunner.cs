using Segres.Tmp;

namespace Segres;

internal class StreamRequestRunner<TResponse> : 
    IRequestConstructor<StreamRequestRunner<TResponse>>
{
    private const string MethodName = nameof(CreateRequestDelegate);
    
    public Type HandlerType { get; }
    private readonly StreamDelegate<TResponse> _handlerMethod;

    public IAsyncEnumerable<TResponse> InvokeHandlerAsync(object handler, IStreamRequest<TResponse> request, CancellationToken cancellationToken, object? interceptors = null)
    {
        return _handlerMethod.Invoke(handler, request, cancellationToken);
    }
    
    public static StreamRequestRunner<TResponse>  Create(Type requestType) => new(requestType);
    
    private StreamRequestRunner(Type requestType)
    {
        var responseType = typeof(TResponse);

        this.HandlerType = typeof(IStreamHandler<,>)
            .MakeGenericType(requestType, responseType);

        this._handlerMethod = (StreamDelegate<TResponse>)this.GetType()
            .CreateInternalDelegate(MethodName, requestType);
    }

    private static StreamDelegate<TResponse> CreateRequestDelegate<TStream>()
        where TStream : IStreamRequest<TResponse>
        => (handler, stream, cancellationToken)
            => ((IStreamHandler<TStream, TResponse>) handler).HandleAsync((TStream) stream, cancellationToken);
}