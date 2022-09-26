using BenchmarkDotNet.Attributes;
using MediatR;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;
using WebApplication1.Models;

namespace ConsoleApp1;

[MemoryDiagnoser(false)]
public class Benchy
{
    private static readonly ServiceProvider ServiceProvider = new ServiceCollection()
        .AddMicrolisR(typeof(Program))
        .AddMediatR(typeof(Program))
        .AddSingleton<ValidationBehavior>()
        .BuildServiceProvider();

    private static readonly IDispatcher Dispatcher = ServiceProvider.GetRequiredService<IDispatcher>();
    private static readonly IMediator Mediator = ServiceProvider.GetRequiredService<IMediator>();
    
    private static readonly WeatherForecastGetById Request = new();

    [Benchmark]
    public async Task<WeatherForecast?> SendAsync_DispatchR()
    {
        var x = await Dispatcher.SendAsync(Request, CancellationToken.None);
        return x;
    }

    [Benchmark]
    public async Task<WeatherForecast?> SendAsync_MediatR()
    {
        var x = await Mediator.Send(Request, CancellationToken.None);
        return x;
    }


    // [Benchmark]
    // public async Task<WeatherForecast> SendAsync_Empty() => await handler.HandleAsync(Request, CancellationToken.None);
}

