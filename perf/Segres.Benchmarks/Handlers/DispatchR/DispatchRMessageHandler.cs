using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRNotificationHandler1 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler1(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler2 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler2(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler3 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler3(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler4 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler4(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler5 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler5(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler6 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler6(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler7 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler7(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler8 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler8(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler9 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler9(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRNotificationHandler10 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRNotificationHandler10(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}