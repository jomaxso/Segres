using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Segres.Abstractions;

namespace Segres;

/// <inheritdoc />
public class SegresConfiguration : ISegresContext
{
    internal static SegresConfiguration? Instance { get; private set; }

    private readonly List<Assembly> _assemblies = new();

    /// <inheritdoc />
    public IEnumerable<Assembly> Assemblies => _assemblies.Distinct().ToArray();

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    internal ServiceLifetime ServiceLifetime { get; private set; } = ServiceLifetime.Scoped;
    internal Type PublisherType { get; private set; } = typeof(Publisher);
    internal bool PublisherStrategy { get; private set; } = false;


    private SegresConfiguration(Assembly assembly, IServiceCollection serviceCollection, Action<SegresConfiguration>? options = null)
    {
        RegisterAssembly(typeof(SegresConfiguration).Assembly)
            .RegisterAssemblies(assembly.AppendReferencedAssemblies())
            .AsScoped();

        Services = serviceCollection;

        options?.Invoke(this);
    }

    internal static SegresConfiguration Create(Assembly assembly, IServiceCollection services, Action<SegresConfiguration>? options = null)
        => Instance ??= new SegresConfiguration(assembly, services, options);

    public SegresConfiguration AsSingleton()
    {
        this.ServiceLifetime = ServiceLifetime.Singleton;
        return this;
    }


    public SegresConfiguration AsScoped()
    {
        this.ServiceLifetime = ServiceLifetime.Scoped;
        return this;
    }


    public SegresConfiguration AsTransient()
    {
        this.ServiceLifetime = ServiceLifetime.Transient;
        return this;
    }

    public SegresConfiguration WithCustomPublisher<TPublisher>()
        where TPublisher : IPublisher =>
        WithCustomPublisher(typeof(TPublisher));

    public SegresConfiguration WithCustomPublisher(Type type)
    {
        if (type.GetInterfaces().All(x => x != typeof(IPublisher) || type.IsAbstract || type.IsInterface))
        {
            throw new Exception("Publisher must implement the IPublisher interface and can not be an interface or an abstract class.");
        }

        PublisherType = type;

        return this;
    }

    public SegresConfiguration WithParallelNotificationHandling(bool asParallel = true)
    {
        PublisherStrategy = asParallel;
        return this;
    }

    public SegresConfiguration RegisterAssembly(Assembly assembly)
    {
        if (!_assemblies.Contains(assembly))
            _assemblies.Add(assembly);

        return this;
    }

    public SegresConfiguration RegisterAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
            RegisterAssembly(assembly);

        return this;
    }
}