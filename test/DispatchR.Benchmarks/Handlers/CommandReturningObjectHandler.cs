using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class CommandReturningObjectHandler : ICommandHandler<CommandReturningObject, object>
{
    public Task<object> HandleAsync(CommandReturningObject request, CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(null);
    }
}