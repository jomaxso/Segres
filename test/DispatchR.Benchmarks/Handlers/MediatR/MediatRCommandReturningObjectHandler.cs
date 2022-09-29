using DispatchR.Benchmarks.Contracts;
using MediatR;

namespace DispatchR.Benchmarks.Handlers;

public class MediatRCommandReturningObjectHandler : IRequestHandler<CreateUserWithResult, int>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRCommandReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task<int> Handle(CreateUserWithResult request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}