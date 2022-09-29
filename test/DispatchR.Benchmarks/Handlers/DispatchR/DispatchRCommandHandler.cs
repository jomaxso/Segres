using DispatchR.Benchmarks.Contracts;
using DispatchR.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class DispatchRCommandHandler : ICommandHandler<CreateUser>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRCommandHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public Task HandleAsync(CreateUser request, CancellationToken cancellationToken)
    {
        return _benchmarkService.RunAsync();
    }
}

