using System.Reflection;
using Segres.Contracts;
using Segres.Internal;
using Segres.Internal.Cache;

namespace Segres;

/// <inheritdoc />
public sealed class Mediator : IMediator
{
    private readonly ServiceResolver _serviceResolver;

    private readonly IHandlerCache<HandlerInfo> _commandHandlerCache;
    private readonly IHandlerCache<HandlerInfo[]> _messageHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _streamHandlerCache;
    private readonly IHandlerCache<HandlerInfo> _queryHandlerCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(ServiceResolver serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).Distinct().ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceResolver"></param>
    /// <param name="markers">The markers for assembly scanning.</param>
    public Mediator(ServiceResolver serviceResolver, ReadOnlySpan<Assembly> markers)
    {
        _serviceResolver = serviceResolver;
        _commandHandlerCache = markers.GetCommandHandlerDetails();
        _queryHandlerCache = markers.GetQueryHandlerDetails();
        _messageHandlerCache = markers.GetEventHandlerDetails();
        _streamHandlerCache = markers.GetStreamHandlerDetails();
    }

    /// <inheritdoc />
    public Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var requestType = command.GetType();
        var handlerInfo = _commandHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<CommandDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, command, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var type = message.GetType();

        if (!_messageHandlerCache.TryGetValue(type, out var handlerInfos))
            return Task.CompletedTask;

        return _serviceResolver.CorePublishAsync(handlerInfos, message, Strategy.WhenAll, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(TMessage message, Strategy strategy, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var type = message.GetType();

        if (!_messageHandlerCache.TryGetValue(type, out var handlerInfos))
            return Task.CompletedTask;

        return _serviceResolver.CorePublishAsync(handlerInfos, message, strategy, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var requestType = query.GetType();
        var handlerInfo = _queryHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<QueryDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, query, cancellationToken);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TResult> CreateStreamAsync<TResult>(IStream<TResult> stream, CancellationToken cancellationToken = default)
    {
        var requestType = stream.GetType();
        var handlerInfo = _streamHandlerCache.FindHandler(requestType);

        var handler = _serviceResolver(handlerInfo.Type)
                      ?? throw new Exception($"No handler registered to handle request of type: {requestType.Name}");

        var handlerDelegate = handlerInfo.ResolveAsyncMethod<StreamDelegate<TResult>>();
        return handlerDelegate.Invoke(handler, stream, cancellationToken);
    }

    /// <inheritdoc />
    public async Task StreamAsync<TResult>(IStream<TResult> stream, Func<TResult, Task> callback, CancellationToken cancellationToken = default)
    {
        var s = this.CreateStreamAsync(stream, cancellationToken);

        await foreach (var item in s.WithCancellation(cancellationToken))
        {
            await callback.Invoke(item);
        }
    }

    /// <inheritdoc />
    public async Task StreamAsync<TResult>(IStream<TResult> stream, Func<TResult, CancellationToken, Task> callback, CancellationToken cancellationToken = default)
    {
        var s = this.CreateStreamAsync(stream, cancellationToken);

        await foreach (var item in s.WithCancellation(cancellationToken))
        {
            await callback.Invoke(item, cancellationToken);
        }
    }
}