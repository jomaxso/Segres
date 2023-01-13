﻿using BenchmarkDotNet.Attributes;
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
    private static readonly CreateUserWithResultSync CreateUserWithResultSync = new(1);
    private static readonly UserCreated UserCreated = new();
    private static readonly UserStreamRequest UserStreamRequest = new();

    private MediatR.IMediator _mediatorMediatR = default!;
    private IMediator _mediatorSegres = default!;
    private IServiceProvider _serviceProvider = default!;

    [Params(1, 10, 100)]
    public int RunNumber { get; set; }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var _ = BenchmarkService.ListOfNumbers;
        _serviceProvider = CreateServiceProvider();

        _mediatorSegres = _serviceProvider.GetRequiredService<Segres.IMediator>();
        _mediatorMediatR = _serviceProvider.GetRequiredService<MediatR.IMediator>();
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<BenchmarkService>();
        services.AddSegres(x =>
        {
            x.UseLifetime(ServiceLifetime.Singleton);
        });

        // services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(FakeValidationBehaviorM<,>));
        // services.AddSingleton(typeof(IRequestBehavior<,>), typeof(FakeValidationBehaviorS<,>));
        // services.AddSingleton(typeof(IRequestBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<Benchmarks>(ServiceLifetime.Singleton);

        services.AddMediatR(x => x.AsSingleton(), typeof(Benchmarks));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorMediator<,>));
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
    public async Task CommandAsync_WithResponse_Segres()
    {
        for (var i = 0; i < RunNumber; i++)
        {
            var x =  await _mediatorSegres.SendAsync(CreateUserWithResult, CancellationToken.None);
        }
    }    
    
    // [Benchmark]
    // public int Command_WithResponseSync_Segres()
    // {
    //     return _sender.Send(CreateUserWithResultSync);
    // }
    
    // [Benchmark]
    // public int Command_WithResponse_Segres()
    // {
    //     return _sender.Send(CreateUserWithResult);
    // }
    
    // [Benchmark]
    // public async Task<int> CommandAsync_WithResponseSync_Segres()
    // {
    //     return await _sender.SendAsync(CreateUserWithResultSync, CancellationToken.None);
    // }

    // [Benchmark]
    // public async Task PublishAsync_Segres() => await _publisher.PublishAsync(UserCreated, CancellationToken.None);
    //
    // [Benchmark]
    // public async Task PublishAsync_Segres_All() => await _publisher.RaiseAsync(UserCreated, PublishStrategy.WhenAll, CancellationToken.Void);
    // //
    // [Benchmark]
    // public async Task PublishAsync_Segres_Sequential() => await _publisher.RaiseAsync(UserCreated, PublishStrategy.Sequential, CancellationToken.Void);
    //
    // [Benchmark]
    // public async Task PublishAsync_Segres_Any() => await _publisher.RaiseAsync(UserCreated, PublishStrategy.WhenAny, CancellationToken.Void);


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
    //     await _mediatorMediatR.Send(CreateUser, CancellationToken.None);
    // }
    //
    [Benchmark]
    public async Task CommandAsync_WithResponse_MediatR()
    {
        for (var i = 0; i < RunNumber; i++)
        {
            var x =  await _mediatorMediatR.Send(CreateUserWithResult, CancellationToken.None);
        }
    }
    //
    // [Benchmark]
    // public async Task PublishAsync_MediatR() => await _mediatorMediatR.Publish(UserCreated, CancellationToken.None);
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