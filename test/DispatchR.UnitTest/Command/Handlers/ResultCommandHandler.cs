using Xunit.Sdk;

namespace DispatchR.UnitTest.Command;

public class ResultCommandHandler :ICommandHandler<ResultCommand, bool>
{
    public async Task<bool> HandleAsync(ResultCommand command, CancellationToken cancellationToken = default)
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