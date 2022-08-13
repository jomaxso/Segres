using MicrolisR;
using MicrolisR.Abstractions;
using MicrolisR.Validation;

namespace Demo;

public class RequestMain : IRequest<bool>, INotification
{
    public int Percentage { get; set; }
}

internal class RequestHandlerMain : IReceiver<RequestMain, bool>
{
    private readonly IValidator _validator;

    public RequestHandlerMain(IValidator validator)
    {
        _validator = validator;
    }

    public Task<bool> ReceiveAsync(RequestMain request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);
        return Task.FromResult(true);
    }
}

internal class Subscriber1 : ISubscriber<RequestMain>
{
    public Task SubscribeAsync(RequestMain message, CancellationToken cancellationToken)
    {
        // Console.WriteLine(nameof(Subscriber1));
        return Task.CompletedTask;
    }
}

public class RandomGuidProvider
{
    public Guid RandomGuid { get; } = Guid.NewGuid();
}