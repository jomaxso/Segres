using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRAsyncNotificationHandler1 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler1(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler2 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler2(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler3 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler3(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler4 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler4(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler5 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler5(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler6 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler6(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler7 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler7(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler8 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler8(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler9 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler9(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class DispatchRAsyncNotificationHandler10 : IAsyncNotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public DispatchRAsyncNotificationHandler10(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}