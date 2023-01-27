using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchREventHandler1 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler1(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler2 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler2(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler3 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler3(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler4 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler4(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler5 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler5(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler6 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler6(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler7 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler7(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler8 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler8(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler9 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler9(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchREventHandler10 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchREventHandler10(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}