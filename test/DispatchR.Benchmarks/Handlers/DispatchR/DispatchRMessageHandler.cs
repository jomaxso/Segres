using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class DispatchRMessageHandler1 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler2 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler3 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler4 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler5 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler6 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler7 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler8 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler9 : IMessageHandler<UserCreated>
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

public class DispatchRMessageHandler10 : IMessageHandler<UserCreated>
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