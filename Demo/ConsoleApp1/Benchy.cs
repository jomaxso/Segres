using BenchmarkDotNet.Attributes;
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
        .AddSingleton<ValidationBehavior>()
        .BuildServiceProvider();

    private static readonly IMediator Mediator1 = ServiceProvider.GetRequiredService<IMediator>();
    private static readonly IMediator Mediator2 = new Mediator(typeof(Program));
    
    private static readonly WeatherForecastGetByIdRequest Request = new();
    private static readonly GetByIdEndpoint handler = ServiceProvider.GetRequiredService<GetByIdEndpoint>();

    [Benchmark]
    public async Task<WeatherForecast?> SendAsync_ASPNET()
    {
        var x = await Mediator1.SendAsync(Request, CancellationToken.None);
        return x;
    }

    [Benchmark]
    public async Task<WeatherForecast?> SendAsync_MANUEL()
    {
        var x = await Mediator2.SendAsync(Request, CancellationToken.None);
        return x;
    }


    // [Benchmark]
    // public async Task<WeatherForecast> SendAsync_Empty() => await handler.HandleAsync(Request, CancellationToken.None);
}

