// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using PlayGround;
using PlayGround.DependencyInjection;

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