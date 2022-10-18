using Segres;
using Segres.Handlers;
using Segres.UnitTest.Event.Objects;
using Xunit.Sdk;

namespace Segres.UnitTest.Event.Handlers;

public class TwoHandlerMessageHandlerTwo : IMessageHandler<TwoHandlerMessage>
{
    public async ValueTask HandleAsync(TwoHandlerMessage message, CancellationToken cancellationToken = default)
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