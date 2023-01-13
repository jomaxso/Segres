using Segres.Handlers;
using Xunit.Sdk;

namespace Segres.UnitTest.Command;

public class ResultCommandHandler : IRequestHandler<ResultCommand, bool>
{
    public async ValueTask<bool> HandleAsync(ResultCommand command, CancellationToken cancellationToken = default)
    {
        switch (command.Number)
        {
            case < 0:
                throw new NotEmptyException();
            case > 0:
                throw new IndexOutOfRangeException();
            default:
                await Task.CompletedTask;
                return true;
        }
    }
}