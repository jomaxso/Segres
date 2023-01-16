using MediatR;

namespace DispatchR.Benchmarks.Handlers.MediatR;

public class MediatRMessageHandler1 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler1(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler2 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler2(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler3 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler3(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler4 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler4(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler5 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler5(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler6 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler6(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler7 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler7(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler8 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler8(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler9 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler9(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}

public class MediatRMessageHandler10 : INotificationHandler<UserCreated>
{
    private readonly BenchmarkService _benchmarkService;

    public MediatRMessageHandler10(BenchmarkService benchmarkService)
    {
        _benchmarkService = benchmarkService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _benchmarkService.RunAsync();
    }
}