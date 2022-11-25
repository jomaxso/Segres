using MediatR;

namespace DispatchR.Benchmarks.Handlers.MediatR;

public class MediatRCommandHandler : IRequestHandler<CreateUser>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRCommandHandler(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task<Unit> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();

        return Unit.Value;
    }
}