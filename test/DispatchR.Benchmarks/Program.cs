// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using DispatchR.Benchmarks;


var b = new Benchmarks();

b.GlobalSetup();

b.SendAsync_Command();

BenchmarkRunner.Run<Benchmarks>();