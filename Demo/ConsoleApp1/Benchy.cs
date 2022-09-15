using BenchmarkDotNet.Attributes;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;
using WebApplication1.Models;

namespace ConsoleApp1;

[MemoryDiagnoser]
public class Benchy
{
    private static IMediator _mediator = new ServiceCollection()
        .AddMicrolisR(typeof(Program))
        .AddSingleton<ValidationBehavior>()
        .BuildServiceProvider()
        .GetRequiredService<IMediator>();


    private static readonly WeatherForecastGetByIdRequest request = new();

    [Benchmark]
    public async Task<WeatherForecast?> SendAsyncWithValidator() => await _mediator.SendAsync(request, CancellationToken.None);
    
    private static readonly WeatherForecastGetByIdRequest2 request2 = new(1);

    [Benchmark]
    public async Task<WeatherForecast?> SendAsyncWithoutValidator() => await _mediator.SendAsync(request2, CancellationToken.None);
}