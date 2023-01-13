using Segres.Handlers;
using Segres.UnitTest.Event.Objects;
using Xunit.Sdk;

namespace Segres.UnitTest.Event.Handlers;

public class TwoHandlerNotificationHandlerTwo : INotificationHandler<TwoHandlerNotification>
{
    public async ValueTask HandleAsync(TwoHandlerNotification notification, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        switch (notification.Number)
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