using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class MessageHandler1 : IMessageHandler<Event>
{
    public Task HandleAsync(Event message, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

public class MessageHandler2 : IMessageHandler<Event>
{
    public Task HandleAsync(Event message, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

public class MessageHandler3 : IMessageHandler<Event>
{
    public Task HandleAsync(Event message, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}


public class MessageHandler4 : IMessageHandler<Event>
{
    public Task HandleAsync(Event message, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}

public class MessageHandler5 : IMessageHandler<Event>
{
    public Task HandleAsync(Event message, CancellationToken cancellationToken)
    {
        return Task.Delay(1000, cancellationToken);
    }
}