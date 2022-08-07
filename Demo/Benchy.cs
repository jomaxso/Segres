using BenchmarkDotNet.Attributes;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;

namespace Demo;

[MemoryDiagnoser()]
public class Benchy
{
    private static readonly ServiceProvider Provider = new ServiceCollection()
        .AddMicrolisR(typeof(Program))
        .BuildServiceProvider();

    private static readonly IMediator Mediator = Provider.GetRequiredService<IMediator>();
    private static readonly ISender Sender = Provider.GetRequiredService<ISender>();
    private static readonly IValidator Validator = Provider.GetRequiredService<IValidator>();
    private static readonly IMapper Mapper = Provider.GetRequiredService<IMapper>();

//     private static PrintMessage obj1 = new("");
//     
//     [Benchmark]
//     public void Validate() => Validator.Validate(obj1);
//
//     [Benchmark]
//     public async Task<bool> MediatAsync() => await Mediator.SendAsync(obj1);
//     
//     [Benchmark]
//     public async Task<bool> SendAsync() => await Sender.SendAsync(obj1);
}