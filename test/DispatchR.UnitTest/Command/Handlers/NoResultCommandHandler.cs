using Xunit.Sdk;

namespace DispatchR.UnitTest.Command;

public class NoResultCommandHandler :ICommandHandler<NoResultCommand>
{
    public async Task HandleAsync(NoResultCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        switch (command.Number)
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