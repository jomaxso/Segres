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
    private static readonly CreateUserWithResultSync CreateUserWithResultSync = new(1);
    private static readonly UserCreated UserCreated = new();
    private static readonly UserStreamRequest UserStreamRequest = new();

    private MediatR.IMediator _mediatorMediatR = default!;
    private IMediator _mediatorSegres = default!;
    private IServiceProvider _serviceProvider = default!;

    // [Params(1, 10, 100)]
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
            x.UseReferencedAssemblies(typeof(Benchmarks));
        });

        services.AddMediatR(x => x.AsSingleton(), typeof(Benchmarks));
        return services.BuildServiceProvider();
    }


    // [Benchmark]
    // public async ValueTask<WeatherForecast?> SendHttpAsync() => await _sender.SendAsync(t, CancellationToken.Void);


    [Benchmark]
    public async Task PublishAsync_Segres() => await _mediatorSegres.PublishAsync(UserCreated, CancellationToken.None);

    [Benchmark]
    public async ValueTask<int> SendAsync_Segres() => await _mediatorSegres.SendAsync(GetUsers, CancellationToken.None);
    


    [Benchmark]
    public async ValueTask CreateStreamAsync_Segres()
    {
        var streamRequest = await _mediatorSegres.SendAsync(UserStreamRequest, CancellationToken.None);
    
        await foreach (var i in streamRequest)
        {
            await ValueTask.CompletedTask;
        }
    }
    
    
    [Benchmark]
    public async Task PublishAsync_MediatR() => await _mediatorMediatR.Publish(UserCreated, CancellationToken.None);
    
    [Benchmark]
    public async ValueTask SendAsync_MediatR() => await _mediatorMediatR.Send(GetUsers, CancellationToken.None);

    [Benchmark]
    public async ValueTask CreateStreamAsync_MediatR()
    {
        var streamRequest = _mediatorMediatR.CreateStream(UserStreamRequest, CancellationToken.None);
    
        await foreach (var i in streamRequest)
        {
            await ValueTask.CompletedTask;
        }
    }
}