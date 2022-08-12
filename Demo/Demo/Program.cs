using Demo;
using Demo.Lib;
using MicrolisR.Abstractions;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


// BenchmarkRunner.Run<Benchy>();
// return;


IServiceProvider serviceProvider = new ServiceCollection()
    .AddMicrolisR<AssemblyMarker, Request>()
    .AddTransient<RandomGuidProvider>()
    .BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();
var succeeded = await mediator.SendAsync(new RequestMain());