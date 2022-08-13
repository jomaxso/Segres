using System.Diagnostics;
using BenchmarkDotNet.Running;
using Demo;
using Demo.Lib;
using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


// BenchmarkRunner.Run<Benchy>();
// return;



IServiceProvider serviceProvider = new ServiceCollection()
    .AddMicrolisR<AssemblyMarker, Request>()
    .AddTransient<RandomGuidProvider>()
    .BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();
int iterations = 0;
var request = new RequestMain();
var duration = TimeSpan.FromSeconds(1);

Stopwatch sw = Stopwatch.StartNew();

while (duration > sw.Elapsed)
{
    iterations++;
    await mediator.SendAsync(request);
} 

sw.Stop();
Console.WriteLine(sw.Elapsed);
Console.WriteLine("Iterations: " + iterations);