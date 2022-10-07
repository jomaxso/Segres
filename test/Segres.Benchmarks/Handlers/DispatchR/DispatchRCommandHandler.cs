using DispatchR.Benchmarks.Contracts;
using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

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

