using BenchmarkDotNet.Attributes;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;
using Microsoft.Extensions.DependencyInjection;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using PrintToConsole;

namespace Demo;

[MemoryDiagnoser()]
public class Benchy
{
    private static readonly IMediator Mediator = new ServiceCollection()
        .AddMicrolisR(typeof(Program))
        .BuildServiceProvider()
        .GetRequiredService<IMediator>();

    private static PrintCommand obj1 = new PrintCommand();
    
    [Benchmark]
    public PrintResult? Map() =>  Mediator.Map(obj1);
    
    [Benchmark]
    public void Validate() => Mediator.Validate(obj1);
    
    [Benchmark]
    public async Task<bool> SendAsync() => await Mediator.SendAsync(obj1);
}