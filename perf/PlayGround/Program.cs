// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using PlayGround;
using PlayGround.DependencyInjection;
using Segres;
using Segres.Contracts;
using Segres.Handlers;

var col = new ServiceCollection();
col.AddSegres(x => x.UseReferencedAssemblies(typeof(Obj)));
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

public class ObjHandler : IRequestHandler<Obj, bool>
{
    public ValueTask<bool> HandleAsync(Obj request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(true);
    }
}

public class ValidationBehavior : RequestBehavior<Obj, bool>
{
    protected override bool Handle(SynchronisedRequestDelegate<bool> next, Obj request)
    {
       return next(request);
    }
}

public class ValidationBehavior1<T1, T2> : RequestBehavior<T1, T2> where T1 : IRequest<T2>
{
    protected override T2 Handle(SynchronisedRequestDelegate<T2> next, T1 request)
    {
        return next(request);
    }
}

public class ValidationBehavior2<T1, T2> : RequestBehavior<T1, T2> where T1 : IRequest<T2>
{
    protected override T2 Handle(SynchronisedRequestDelegate<T2> next, T1 request)
    {
        return next(request);
    }
}
