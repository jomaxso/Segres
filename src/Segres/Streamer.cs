using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Segres;

/// <inheritdoc />
internal sealed class Streamer : IStreamer
{
    private readonly  IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _streamHandlerCache = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public Streamer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStreamRequest<TResult> streamRequest, CancellationToken cancellationToken = default)
    {
        var requestType = streamRequest.GetType();
        var handlerInfo = _streamHandlerCache.GetOrAdd<StreamRequestRunner<TResult>>(requestType);

        var handler = _serviceProvider.GetService(handlerInfo.HandlerType)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        return handlerInfo.InvokeHandlerAsync(handler, streamRequest, cancellationToken);
    }
}