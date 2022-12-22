using System.Reflection;

namespace Segres;

/// <summary>
/// 
/// </summary>
public class SegresConfiguration
{
    private readonly List<Assembly> _assemblies = new();

    internal IEnumerable<Assembly> Assemblies => _assemblies.Distinct().ToArray();

    internal HandlerLifetime Lifetime { get; private set; } = HandlerLifetime.Scoped;
    internal Type PublisherType { get; private set; } = typeof(Publisher);
    internal bool PublisherStrategy { get; private set; } = false;

    /// <summary>
    /// 
    /// </summary>
    private SegresConfiguration(Assembly assembly, Action<SegresConfiguration>? options = null)
    {
        RegisterAssembly(typeof(SegresConfiguration).Assembly)
            .RegisterAssemblies(assembly.AppendReferencedAssemblies())
            .AsScoped();
        
        options?.Invoke(this);
    }

    internal static SegresConfiguration Create(Assembly assembly, Action<SegresConfiguration>? options = null) 
        => new(assembly, options);

    /// <inheritdoc />
    public SegresConfiguration AsSingleton()
    {
        this.Lifetime = HandlerLifetime.Singleton;
        return this;
    }
    
    /// <inheritdoc />
    public SegresConfiguration AsScoped()
    {
        this.Lifetime = HandlerLifetime.Scoped;
        return this;
    }
    
    /// <inheritdoc />
    public SegresConfiguration AsTransient()
    {
        this.Lifetime = HandlerLifetime.Transient;
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

    public SegresConfiguration WithParallelNotificationHandling(bool asParallel = true)
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