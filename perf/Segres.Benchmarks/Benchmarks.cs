using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using DispatchR.Benchmarks;
using DispatchR.Benchmarks.Handlers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Alphabetical)]
public class Benchmarks
{
    private static readonly GetUsers GetUsers = new();

    private static readonly CreateUser CreateUser = new();
    private static readonly CreateUserWithResult CreateUserWithResult = new(1);
    private static readonly UserCreated UserCreated = new();
    private static readonly UserStreamRequest UserStreamRequest = new();

    private IMediator _mediatorMediatR = default!;
    private IPublisher _publisher = default!;
    private ISender _sender = default!;
    private IServiceProvider _serviceProvider = default!;
    private IStreamer _streamer = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var _ = BenchmarkService.ListOfNumbers;
        _serviceProvider = CreateServiceProvider();

        _sender = _serviceProvider.GetRequiredService<ISender>();
        _publisher = _serviceProvider.GetRequiredService<IPublisher>();
        _streamer = _serviceProvider.GetRequiredService<IStreamer>();
        _mediatorMediatR = _serviceProvider.GetRequiredService<IMediator>();
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<BenchmarkService>();
        services.AddSegres(x =>
        {
            x.AsSingleton();
            // x.RegisterAssembly(typeof(Benchmarks).Assembly);
        });

        services.AddSingleton(typeof(IRequestBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<Benchmarks>();

        services.AddMediatR(x => x.AsSingleton(), typeof(Benchmarks));
        return services.BuildServiceProvider();
    }

    // [Benchmark]
    // public async ValueTask<WeatherForecast?> SendHttpAsync() => await _sender.SendAsync(t, CancellationToken.Void);

    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_Segres()
    // {
    //      await _sender.SendAsync(CreateUser, CancellationToken.Void);
    // }
    //
    [Benchmark]
    public async Task<int> CommandAsync_WithResponse_Segres()
    {
        return await _sender.SendAsync(CreateUserWithResult, CancellationToken.None);
    }

    // [Benchmark]
    // public async Task PublishAsync_Segres() => await _publisher.PublishAsync(UserCreated, CancellationToken.Void);
    //
    // [Benchmark]
    // public async Task PublishAsync_Segres_All() => await _publisher.PublishAsync(UserCreated, PublishStrategy.WhenAll, CancellationToken.Void);
    // //
    // [Benchmark]
    // public async Task PublishAsync_Segres_Sequential() => await _publisher.PublishAsync(UserCreated, PublishStrategy.Sequential, CancellationToken.Void);
    //
    // [Benchmark]
    // public async Task PublishAsync_Segres_Any() => await _publisher.PublishAsync(UserCreated, PublishStrategy.WhenAny, CancellationToken.Void);


    // [Benchmark]
    // public async ValueTask<int> QueryAsync_WithResponse_Sender_Segres() => await _sender.SendAsync(GetUsers, CancellationToken.Void);

    // private readonly Predicate<int?> f = (x) => x == 0;
    //
    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_Segres_Where()
    // {
    //     var streamRequest = _streamer.CreateStreamAsync(UserStreamRequest, CancellationToken.Void)
    //         .Where(f);
    //
    //     await foreach (var i in streamRequest)
    //     {
    //         await ValueTask.CompletedTask;
    //     }
    // }

    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_Segres()
    // {
    //     var streamRequest = _streamer.CreateStreamAsync(UserStreamRequest, CancellationToken.Void);
    //
    //     await foreach (var i in streamRequest)
    //     {
    //         await ValueTask.CompletedTask;
    //     }
    // }

    // [Benchmark]
    // public async Task CommandAsync_WithoutResponse_MediatR()
    // {
    //     await _mediatorMediatR.Send(CreateUser, CancellationToken.Void);
    // }
    //
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponse_MediatR() => await _mediatorMediatR.Send(CreateUserWithResult, CancellationToken.Void);
    //
    // // [Benchmark]
    // // public async Task PublishAsync_MediatR() => await _mediatorMediatR.Publish(UserCreated, CancellationToken.Void);
    //
    // [Benchmark]
    // public async ValueTask QueryAsync_WithResponse_MediatR() => await _mediatorMediatR.Send(GetUsers, CancellationToken.Void);

    // [Benchmark]
    // public async ValueTask CreateStreamAsync_WithResponse_MediatR()
    // {
    //     var streamRequest = _mediatorMediatR.CreateStream(UserStreamRequest, CancellationToken.Void);
    //
    //     await foreach (var i in streamRequest)
    //     {
    //         await ValueTask.CompletedTask;
    //     }
    // }
}