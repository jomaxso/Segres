// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using PlayGround;
using PlayGround.DependencyInjection;
using Segres;
using Segres.Abstractions;

var col = new ServiceCollection();
col.AddSegres();
ServiceProvider _provider = col.BuildServiceProvider();

var s = _provider.GetRequiredService<ISender>();

var b = s.Send(new Obj());
b = s.Send(new Obj());

    
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

public record Obj : IRequest<bool>;

public class ObjHandler : IAsyncRequestHandler<Obj, bool>
{
    public ValueTask<bool> HandleAsync(Obj request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(true);
    }
}

public class ValidationBehavior : IRequestBehavior<Obj, bool>
{
    public bool Handle(RequestHandlerDelegate<bool> next, Obj request)
    {
       return next(request);
    }
}

public class ValidationBehavior1<T1, T2> : IRequestBehavior<T1, T2> where T1 : IRequest<T2>
{
    public T2 Handle(RequestHandlerDelegate<T2> next, T1 request)
    {
        return next(request);
    }
}

public class ValidationBehavior2<T1, T2> : IRequestBehavior<T1, T2> where T1 : IRequest<T2>
{
    public T2 Handle(RequestHandlerDelegate<T2> next, T1 request)
    {
        return next(request);
    }
}
