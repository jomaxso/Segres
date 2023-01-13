using System.Runtime.CompilerServices;
using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

public static class MediatorExtensions
{
    /// <summary>
    /// Synchronously send a request to a single handler. 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="request">The request object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Send(this ISender sender, IRequest request) 
        => sender.SendAsync(request, CancellationToken.None).Await();

    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="IRequestHandler{TRequest}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask SendAsync(this ISender sender, IRequest request, CancellationToken cancellationToken = default)
        => await sender.SendAsync(request, cancellationToken);


    /// <summary>
    /// Synchronously send a request to a single handler. 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="request">The request object.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The result of the executed handler.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResponse Send<TResponse>(this ISender sender, IRequest<TResponse> request)
        => sender.SendAsync(request, CancellationToken.None).Await();

    /// <summary>
    /// Synchronously send a stream to a single handler. 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>The result of the executed handler.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAsyncEnumerable<TResponse> Send<TResponse>(this ISender sender, IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        => sender.SendAsync(request, cancellationToken).Await();

    /// <summary>
    /// Asynchronously receive a stream from a single handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="request"></param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A streamRequest as <see cref="IAsyncEnumerable{T}"/>.</returns>
    /// <seealso cref="IStreamRequestHandler{TRequest,TResponse}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<IAsyncEnumerable<TResult>> SendAsync<TResult>(this ISender sender, IStreamRequest<TResult> request, CancellationToken cancellationToken = default)
        => sender.SendAsync(request, cancellationToken);
    
    /// <summary>
    /// Synchronously send a notification to multiple subscribers.
    /// </summary>
    /// <param name="publisher">The publisher.</param>
    /// <param name="notification">The notification object</param>
    /// <returns>A task that represents the publish operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Publish<TNotification>(this IPublisher publisher, TNotification notification)
        where TNotification : INotification 
        => publisher.PublishAsync(notification, CancellationToken.None).Await();

    private static void Await(this ValueTask valueTask)
    {
        if (valueTask.IsCompleted)
            return;

        valueTask.AsTask()
            .GetAwaiter()
            .GetResult();
    }
    
    private static TResult Await<TResult>(this ValueTask<TResult> valueTask)
    {
        if (valueTask.IsCompleted)
            return valueTask.Result;

        return valueTask.AsTask()
            .GetAwaiter()
            .GetResult();
    }
}