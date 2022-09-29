using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class DispatchRQueryReturningObjectHandler : IQueryHandler<GetUsers, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRQueryReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task<int> HandleAsync(GetUsers request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}