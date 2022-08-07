using MicrolisR;

namespace Demo.Domain.PrintToConsole;

public record PrintMessage(string Value) : IMessage;

public class PrintMessageHandler : IMessageHandler<PrintMessage>
{
    public Task SubscribeAsync(PrintMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine(message.Value + " 1");

        return Task.CompletedTask;
    }
}

public class PrintMessageHandler2 : IMessageHandler<PrintMessage>
{
    public Task SubscribeAsync(PrintMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine(message.Value + " 2");

        return Task.CompletedTask;
    }
}
