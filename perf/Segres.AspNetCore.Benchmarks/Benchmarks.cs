using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Segres;
using Segres.AspNetCore;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Alphabetical)]
public class Benchmarks
{
    private Test request1 = new();

    private IServiceProvider _serviceProvider;
    private ISender _sender;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _serviceProvider = CreateServiceProvider();
        _sender = _serviceProvider.GetRequiredService<ISender>();
    }

    private static EndpointRequest Request = new EndpointRequest();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSegres(ServiceLifetime.Singleton);
        return services.BuildServiceProvider();
    }

    [Benchmark()]
    public async Task ResolveAsync()
    {
        var x = await _sender.SendAsync(request1, CancellationToken.None);
    }
}

public record class EndpointRequest : IHttpRequest<bool>
{
    public static string RequestRoute => default!;
    public static RequestType RequestType => default!;
}

public class Endpoint : AbstractEndpoint<EndpointRequest, bool>
{
    private static HttpResult<bool> result = new HttpResult<bool>(true);

    public override async ValueTask<HttpResult<bool>> ResolveAsync(EndpointRequest request, CancellationToken cancellationToken)
    {
        await ValueTask.CompletedTask;
        return result;
    }
}