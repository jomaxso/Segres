using BenchmarkDotNet.Attributes;
using DispatchR.Benchmarks.Contracts;
using DispatchR.Extensions.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace DispatchR.Benchmarks;

/// <summary>
/// 
/// </summary>
[MemoryDiagnoser(false)]
public class Benchmarks
{
    private IDispatcher _dispatcher = default!;

    private static readonly QueryReturningObject QueryReturningObject = new();
    private static readonly Command Command = new();
    private static readonly CommandReturningObject CommandReturningObject = new();


    [GlobalSetup]
    public void GlobalSetup()
    {
        var serviceProvider = new ServiceCollection()
            .AddDispatchR(typeof(Benchmarks))
            .BuildServiceProvider();

        this._dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
    }

    [Benchmark]
    public Task<object> SendAsync_Query_WithResponse() => _dispatcher.SendAsync(QueryReturningObject, CancellationToken.None);

    [Benchmark]
    public Task SendAsync_Command() => _dispatcher.SendAsync(Command, CancellationToken.None);

    [Benchmark]
    public Task<object> SendAsync_Command_WithResponse() => _dispatcher.SendAsync(CommandReturningObject, CancellationToken.None);
}

