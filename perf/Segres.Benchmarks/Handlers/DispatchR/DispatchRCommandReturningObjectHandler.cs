using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRAsyncCommandReturningObjectHandler : IRequestHandler<CreateUserWithResult, int>
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
public class DispatchRAsyncCommandReturningObjectSyncHandler : RequestHandler<CreateUserWithResultSync, int>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncCommandReturningObjectSyncHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    protected override int Handle(CreateUserWithResultSync request)
    {
       return _benchmarkService.Run();
    }
}