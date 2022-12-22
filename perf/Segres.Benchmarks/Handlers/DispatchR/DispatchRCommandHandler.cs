using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRCommandHandler : IAsyncRequestHandler<CreateUser>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRCommandHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(CreateUser request, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}