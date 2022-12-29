// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using PlayGround;
using PlayGround.DependencyInjection;
using Segres;


ServiceProvider _provider = new ServiceCollection()
    .AddSegres()
    .BuildServiceProvider();

return;
BenchmarkRunner.Run<Benchmarks>();
return;

Console.WriteLine("Hello, World!");

var container = new SegresCollection()
    .RegisterSingleton<ISomeService, SomeServiceOne>()
    .RegisterSingleton<IRandomGuidGenerator, RandomGuidGenerator>()
    .BuildContainer();

var serviceFirst = container.GetService<ISomeService>();
var serviceSecond = container.GetService<ISomeService>();

serviceFirst.PrintSomething();
serviceSecond.PrintSomething();
