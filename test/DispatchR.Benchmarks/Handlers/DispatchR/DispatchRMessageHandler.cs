using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class DispatchRMessageHandler1 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler1(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler2 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler2(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler3 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler3(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler4 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler4(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler5 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler5(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler6 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler6(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler7 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler7(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler8 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler8(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler9 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler9(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRMessageHandler10 : IEventHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRMessageHandler10(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task HandleAsync(UserCreated message, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}