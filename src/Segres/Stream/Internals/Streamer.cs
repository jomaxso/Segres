using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

internal class Streamer : IStreamer
{
    private readonly IServiceProvider _serviceProvider;
    
    private readonly ConcurrentDictionary<Type, object> _streamHandlerCache = new();

    public Streamer(IServiceProvider serviceProvider, SegresConfiguration? options = null)
    {
        _serviceProvider = options?.Lifetime is null or ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider
            : serviceProvider;
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