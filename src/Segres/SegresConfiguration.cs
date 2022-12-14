using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <summary>
/// 
/// </summary>
public class SegresConfiguration
{
    private readonly List<Assembly> _assemblies = new();

    internal IEnumerable<Assembly> Assemblies => _assemblies.Distinct().ToArray();

    internal ServiceLifetime Lifetime { get; private set; } = ServiceLifetime.Scoped;
    internal Type PublisherType { get; private set; } = typeof(Publisher);
    internal bool PublisherStrategy { get; private set; } = false;

    /// <summary>
    /// 
    /// </summary>
    private SegresConfiguration()
    {
        WithHandlerLifetime(ServiceLifetime.Scoped);
        RegisterAssembly(typeof(ISender).Assembly);
    }

    internal static SegresConfiguration Create() => new();

    /// <inheritdoc />
    public SegresConfiguration WithHandlerLifetime(ServiceLifetime lifetime)
    {
        this.Lifetime = lifetime;
        return this;
    }

    public SegresConfiguration WithCustomPublisher<TPublisher>()
        where TPublisher : IPublisher =>
        WithCustomPublisher(typeof(TPublisher));

    public SegresConfiguration WithCustomPublisher(Type type)
    {
        if (type.GetInterfaces().All(x => x != typeof(IPublisher) || type.IsAbstract || type.IsInterface))
        {
            throw new Exception();
        }

        PublisherType = type;

        return this;
    }

    public SegresConfiguration WithParallelPublishing(bool asParallel = true)
    {
        PublisherStrategy = asParallel;
        return this;
    }

    /// <inheritdoc />
    public SegresConfiguration RegisterAssembly(Assembly assembly)
    {
        if (!_assemblies.Contains(assembly))
            _assemblies.Add(assembly);

        return this;
    }

    /// <inheritdoc />
    public SegresConfiguration RegisterAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
            RegisterAssembly(assembly);

        return this;
    }

    internal List<(string, Type)> l = new();

    public SegresConfiguration WithHttpClient<TRequest>(string baseAddress)
    {
        l.Add((baseAddress, typeof(Client<,>).MakeGenericType(typeof(TRequest), typeof(object))));
        return this;
    }
}

internal sealed class Client<TRequest, TResponse>
{
    private readonly HttpClient _httpClient;
    
    public Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}