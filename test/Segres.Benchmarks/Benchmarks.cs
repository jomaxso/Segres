using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using DispatchR.Benchmarks.Contracts;
using DispatchR.Benchmarks.Handlers;
using Segres.Extensions.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using MediatR;


namespace DispatchR.Benchmarks;

/// <summary>
/// 
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Alphabetical)]
public class Benchmarks
{
    private Segres.IMediator _dispatcher = default!;
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
            .AddSegres<Benchmarks>()
            .AddMediatR(typeof(Benchmarks))
            .BuildServiceProvider();

        this._dispatcher = _serviceProvider.GetRequiredService<Segres.IMediator>();
        this._mediator = _serviceProvider.GetRequiredService<MediatR.IMediator>();
    }

    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_DispatchR() => await _dispatcher.CommandAsync(CreateUser, CancellationToken.None);
    //
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponse_DispatchR() => await _dispatcher.CommandAsync(CreateUserWithResult, CancellationToken.None);

    
    [Benchmark]
    public async Task PublishAsync_DispatchR() => await _dispatcher.PublishAsync(UserCreated, Segres.Strategy.WhenAll, CancellationToken.None);
    
    [Benchmark]
    public async Task PublishAsync_DispatchR_Any() => await _dispatcher.PublishAsync(UserCreated, Segres.Strategy.WhenAny, CancellationToken.None);
    //
    // [Benchmark]
    // public Task<int> Querysync_WithResponse_DispatchR() => _dispatcher.QueryAsync(GetUsers, CancellationToken.None);
    //
    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_DispatchR()
    // {
    //     var stream = _dispatcher.CreateStreamAsync(UserStream, CancellationToken.None);
    //
    //     await foreach (var i in stream)
    //     {
    //     }
    // }
    //
    // [Benchmark]
    // public async ValueTask StreamAsync_WithResponse_DispatchR() => await _dispatcher.StreamAsync(UserStream, _ => ValueTask.CompletedTask, CancellationToken.None);


    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_MediatR() => await _mediator.Send(CreateUser, CancellationToken.None);
    //
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponse_MediatR() => await _mediator.Send(CreateUserWithResult, CancellationToken.None);

    [Benchmark]
    public async Task PublishAsync_MediatR() => await _mediator.Publish(UserCreated, CancellationToken.None);
    //
    // [Benchmark]
    // public Task<int> QueryAsync_WithResponse_MediatR() => _mediator.Send(GetUsers, CancellationToken.None);
    //
    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_MediatR()
    // {
    //     var stream = _mediator.CreateStream(UserStream, CancellationToken.None);
    //
    //     await foreach (var i in stream)
    //     {
    //         
    //     }
    // }
}