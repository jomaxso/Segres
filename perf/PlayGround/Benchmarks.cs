using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using PlayGround.DependencyInjection;

namespace PlayGround;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    private readonly SegresContainer _container = new SegresCollection()
        .RegisterTransient<ISomeService, SomeServiceOne>()
        .RegisterTransient<IRandomGuidGenerator, RandomGuidGenerator>()
        .BuildContainer();

    private readonly ServiceProvider _provider = new ServiceCollection()
        .AddTransient<ISomeService, SomeServiceOne>()
        .AddTransient<IRandomGuidGenerator, RandomGuidGenerator>()
        .BuildServiceProvider();

    [Benchmark]
    public void GetServiceOfTAndT_Segres()
    {
        var serviceFirst = _container.GetService<ISomeService>();
    }
    
    [Benchmark]
    public void GetServiceOfTAndT_Microsoft()
    {
        var serviceFirst = _provider.GetService<ISomeService>();
    }
}