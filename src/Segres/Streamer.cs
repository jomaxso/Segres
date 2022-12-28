using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Definitions;
using Segres.Delegates;
using Segres.Extensions;

namespace Segres;

public sealed class Streamer : IStreamer
{
    private readonly ServiceResolver _serviceResolver;
    private readonly ConcurrentDictionary<Type, object> _streamHandlerCache;
    
    public Streamer(ServiceResolver serviceResolver, IDictionary<Type, object> handlers)
    {
        _serviceResolver = serviceResolver;
        _streamHandlerCache = new ConcurrentDictionary<Type, object>(handlers);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStreamRequest<TResult> streamRequest, CancellationToken cancellationToken = default)
    {
        var requestType = streamRequest.GetType();
        var handlerInfo = _streamHandlerCache.GetOrAdd<StreamRequestRunner<TResult>>(requestType);

        var handler = _serviceResolver.GetService(handlerInfo.HandlerType)
                      ?? throw new Exception($"No handler registered to handle asyncRequest of type: {requestType.Name}");

        return handlerInfo.InvokeHandlerAsync(handler, streamRequest, cancellationToken);
    }
}