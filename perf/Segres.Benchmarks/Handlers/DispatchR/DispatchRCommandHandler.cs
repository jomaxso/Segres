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

    public async Task HandleAsync(CreateUser request, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

