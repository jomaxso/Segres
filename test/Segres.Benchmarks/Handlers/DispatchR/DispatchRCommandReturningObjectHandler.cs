using DispatchR.Benchmarks.Contracts;
using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRAsyncCommandReturningObjectHandler : ICommandHandler<CreateUserWithResult, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncCommandReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }


    public async Task<int> HandleAsync(CreateUserWithResult request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}