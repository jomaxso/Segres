// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using DispatchR.Benchmarks;


var b = new Benchmarks();

b.GlobalSetup();

b.PublishAsync();

BenchmarkRunner.Run<Benchmarks>();