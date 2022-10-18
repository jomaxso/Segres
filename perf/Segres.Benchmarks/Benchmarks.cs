using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using DispatchR.Benchmarks.Contracts;
using DispatchR.Benchmarks.Handlers;
using DispatchR.Benchmarks.Handlers.DispatchR;
using Segres.Extensions.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Alphabetical)]
public class Benchmarks
{
    private Segres.IServiceBroker _serviceBroker = default!;
    private MediatR.IMediator _mediator = default!;
    private IServiceProvider _serviceProvider = default!;

    private static readonly GetUsers GetUsers = new();
    private static readonly CreateUser CreateUser = new();
    private static readonly CreateUserWithResult CreateUserWithResult = new();
    private static readonly UserCreated UserCreated = new();
    private static readonly UserStream UserStream = new();

    [GlobalSetup]
    public void GlobalSetup()
    {
        _ = BenchmarkService.ListOfNumbers;
        this._serviceProvider = new ServiceCollection()
            .AddSingleton<BenchmarkService>()
            .AddSegres<Benchmarks>(x => x.AsSingleton())
            .AddMediatR(x => x.AsSingleton(), typeof(Benchmarks))
            .BuildServiceProvider();

        this._serviceBroker = _serviceProvider.GetRequiredService<Segres.IServiceBroker>();
        this._mediator = _serviceProvider.GetRequiredService<MediatR.IMediator>();
    }

    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_DispatchR() => await _serviceBroker.SendAsync(CreateUser, CancellationToken.None);
    //
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponse_DispatchR() => await _serviceBroker.SendAsync(CreateUserWithResult, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_DispatchR() => await _serviceBroker.PublishAsync(UserCreated, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_DispatchR_All() => await _serviceBroker.PublishAsync(UserCreated, Strategy.WhenAll, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_DispatchR_Sequential() => await _serviceBroker.PublishAsync(UserCreated, Segres.Strategy.Sequential, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_DispatchR_Any() => await _serviceBroker.PublishAsync(UserCreated, Segres.Strategy.WhenAny, CancellationToken.None);

    // [Benchmark]
    // public async Task PublishAsync_DispatchR_Any() => await _serviceBroker.PublishAsync(UserCreated, Segres.Strategy.WhenAny, CancellationToken.None);

    // [Benchmark]
    // public Task<int> Querysync_WithResponse_DispatchR() => _serviceBroker.SendAsync(GetUsers, CancellationToken.None);

    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_DispatchR()
    // {
    //     var stream = _serviceBroker.CreateStreamAsync(UserStream, CancellationToken.None);
    //
    //     await foreach (var i in stream)
    //     {
    //         await ValueTask.CompletedTask;
    //     }
    // }

    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_MediatR() => await _mediator.Send(CreateUser, CancellationToken.None);
    //
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponse_MediatR() => await _mediator.Send(CreateUserWithResult, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_MediatR() => await _mediator.Publish(UserCreated, CancellationToken.None);

    // [Benchmark]
    // public Task<int> QueryAsync_WithResponse_MediatR() => _mediator.Send(GetUsers, CancellationToken.None);

    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_MediatR()
    // {
    //     var stream = _mediator.CreateStream(UserStream, CancellationToken.None);
    //
    //     await foreach (var i in stream)
    //     {
    //         await ValueTask.CompletedTask;
    //     }
    // }
}