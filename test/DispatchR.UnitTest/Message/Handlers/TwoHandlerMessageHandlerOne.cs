using DispatchR.UnitTest.Event.Objects;
using Xunit.Sdk;

namespace DispatchR.UnitTest.Event.Handlers;

public class TwoHandlerMessageHandlerOne : IMessageHandler<TwoHandlerMessage>
{
    public async Task HandleAsync(TwoHandlerMessage message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        switch (message.Number)
        {
            case < 0:
                throw new NotEmptyException();
            case > 0:
                throw new IndexOutOfRangeException();
            default:
                return;
        }
    }
}