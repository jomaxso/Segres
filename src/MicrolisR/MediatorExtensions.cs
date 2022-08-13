using MicrolisR.Validation;

namespace MicrolisR;

/// <summary>
/// Extension methods for the <see cref="IMediator"/> interface.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// The request and response will be validated through the <see cref="IValidator"/> before and after sending.
    /// </summary>
    /// <param name="mediator">The mediator</param>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    /// <seealso cref="IReceiver{T, T}"/>
    public static async Task<TResponse> SendValidAsync<TResponse>(this IMediator mediator, IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        mediator.Validate(request);
        
        var response = await mediator.SendAsync(request, cancellationToken);

        if (response is IValidatable validatable)
            mediator.Validate(validatable);

        return response;
    }

    /// <summary>
    /// Asynchronously send a request to a single Receiver.
    /// The request will be validated through the <see cref="IValidator"/> before sending.
    /// </summary>
    /// <param name="mediator">The mediator</param>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the send operation.</returns>
    /// <seealso cref="IReceiver{T}"/>
    public static Task SendValidAsync(this IMediator mediator, IRequest request, CancellationToken cancellationToken = default)
    {
        mediator.Validate(request);
        return mediator.SendAsync(request, cancellationToken);
    }


    /// <summary>
    /// Asynchronously send a notification to multiple subscribers.
    /// The notification will be validated through the <see cref="IValidator"/> before sending.
    /// </summary>
    /// <param name="mediator">The mediator</param>
    /// <param name="notification">The notification object</param>
    /// <param name="cancellationToken">An optional cancellation token</param>
    /// <returns>A task that represents the publish operation.</returns>
    public static Task PublishValidAsync(this IMediator mediator, INotification notification, CancellationToken cancellationToken = default)
    {
        mediator.Validate(notification);
        return mediator.PublishAsync(notification, cancellationToken);
    }
}

