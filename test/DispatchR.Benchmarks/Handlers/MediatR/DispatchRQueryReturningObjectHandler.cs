using DispatchR.Benchmarks.Contracts;
using MediatR;

namespace DispatchR.Benchmarks.Handlers;

public class MediatRQueryReturningObjectHandler : IRequestHandler<GetUsers, int>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRQueryReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task<int> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}