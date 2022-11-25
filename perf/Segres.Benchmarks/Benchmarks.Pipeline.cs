using System.Collections.Concurrent;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.Benchmarks;

public sealed class ThePersonHandler : IRequestHandler<GetPersonAgeQuery, int>
{
    public async ValueTask<int> HandleAsync(GetPersonAgeQuery request, CancellationToken cancellationToken = default)
    {
        await ValueTask.CompletedTask;
        return request.Age;
    }
}

public sealed class ThePersonHandlerValidatorOne : IRequestBehavior<GetPersonAgeQuery, int>
{
    public ValueTask<int> HandleAsync(RequestDelegate<int> next, GetPersonAgeQuery request, CancellationToken cancellationToken)
    {
        return next(request, cancellationToken);
    }
}

[MemoryDiagnoser(false)]
public class BenchmarksPipeline
{
    private static readonly IServiceProvider Provider = new ServiceCollection()
        .AddSegres()
        // .AddSingleton<IRequestBehavior<GetPersonAgeQuery, int>, ThePersonHandlerValidatorOne>()
        .AddSingleton<IRequestBehavior<GetPersonAgeQuery, int>, ThePersonHandlerValidatorOne>()
        .AddSingleton<ThePersonHandlerValidatorOne>()
        .BuildServiceProvider();

    private static Type type = typeof(IRequestBehavior<GetPersonAgeQuery, int>);

    private static RequestDelegate<int> Fu = async (r, c) =>
    {
        await Task.Delay(1);
        return ((GetPersonAgeQuery) r).Age;
    };

    private static readonly GetPersonAgeQuery p = new(0);


    // [Benchmark]
    // public async ValueTask<int> PipeMe1()
    // {
    //     var handlers = Provider.GetServices(type);
    //     var @delegate = RequestInterceptorExtensions.Create<GetPersonAgeQuery, int>(handlers!, Fu);
    //     return await @delegate.Invoke(p, CancellationToken.Void);
    // }
    //
    // [Benchmark]
    // public async ValueTask<int> PipeMeSegres()
    // {
    //     return await _sender.SendAsync(p, CancellationToken.Void);
    // }

    private static readonly ConcurrentDictionary<Type, KeyValuePair<Type, Delegate>> d = new();

    private static readonly GetPersonAgeQuery o = new();

    private readonly ISender _sender = Provider.GetRequiredService<ISender>();

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public async ValueTask MakeType()
    {
        var x = await SendAsync(o);
    }

    private ValueTask<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        var x = d.GetOrAdd(request.GetType(), Register<T>);

        return ((Func<object, IRequest<T>, CancellationToken, ValueTask<T>>) x.Value)
            .Invoke(Provider.GetService(x.Key)!, request, cancellationToken);
    }

    private static KeyValuePair<Type, Delegate> Register<T>(Type t)
    {
        var type1 = typeof(IRequestHandler<,>).MakeGenericType(t, typeof(T));
        var del = CreateDelegate<T>(t);
        return new KeyValuePair<Type, Delegate>(type1, del);
    }

    private static Func<object, IRequest<T>, CancellationToken, ValueTask<T>> CreateDelegate<T>(Type requestType)
    {
        var method = typeof(BenchmarksPipeline).GetMethod(nameof(ExecuteAsync), BindingFlags.Static | BindingFlags.NonPublic);
        var genericMethod = method!.MakeGenericMethod(requestType, typeof(T)).Invoke(null, Array.Empty<object>())!;
        return (Func<object, IRequest<T>, CancellationToken, ValueTask<T>>) genericMethod;
    }

    private static Func<object, IRequest<T>, CancellationToken, ValueTask<T>> ExecuteAsync<TRequest, T>()
        where TRequest : IRequest<T>
    {
        return (obj, request, cancellationToken) => ((IRequestHandler<TRequest, T>) obj).HandleAsync((TRequest) request, cancellationToken);
    }
}