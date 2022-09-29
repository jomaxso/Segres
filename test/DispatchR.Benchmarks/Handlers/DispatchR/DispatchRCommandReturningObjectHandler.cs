using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

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