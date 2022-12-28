using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRAsyncCommandReturningObjectHandler : IAsyncRequestHandler<CreateUserWithResult, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncCommandReturningObjectHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }


    public async ValueTask<int> HandleAsync(CreateUserWithResult request, CancellationToken cancellationToken)
    {
        return await _benchmarkService.RunAsync();
    }
}
public class DispatchRAsyncCommandReturningObjectSyncHandler : IRequestHandler<CreateUserWithResultSync, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncCommandReturningObjectSyncHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public int Handle(CreateUserWithResultSync request)
    {
       return _benchmarkService.Run();
    }
}