using BenchmarkDotNet.Attributes;
using Demo.Lib;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using MicrolisR.Validation;
using Microsoft.Extensions.DependencyInjection;


namespace Demo;

[MemoryDiagnoser]
public class Benchy
{
    private static readonly IServiceProvider ServiceProvider = new ServiceCollection()
        .AddMicrolisR(AssemblyMarker.Assembly, typeof(Request).Assembly)
        .AddTransient<RandomGuidProvider>()
        .BuildServiceProvider();

    private static readonly IMediator Mediator = ServiceProvider.GetRequiredService<IMediator>();
    private static readonly IValidator Validator = ServiceProvider.GetRequiredService<IValidator>();
    private static readonly Request Request = new();
    
    
    private static readonly RequestMain RequestMain = new();
    




    // [Benchmark]
    // public async Task<bool> SendAsync() => await Mediator.SendAsync(Request, CancellationToken.None);
    
    // [Benchmark]
    // public async Task<bool> TrySendAsync() => await Mediator.TrySendAsync(Request, CancellationToken.None);
    
    
    [Benchmark]
    public void Validate() => Validator.Validate(RequestMain);
    
    // [Benchmark]
    // public async ValueTask PublishAsync() => await Mediator.PublishAsync(RequestMain, CancellationToken.None);
}
