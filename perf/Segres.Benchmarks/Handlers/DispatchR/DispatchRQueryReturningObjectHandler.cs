using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRQueryReturningObjectHandler : IRequestHandler<GetUsers, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRQueryReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask<int> HandleAsync(GetUsers request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}
