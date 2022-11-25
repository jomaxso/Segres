using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <summary>
/// 
/// </summary>
public record RegistrationOption
{
    private readonly List<Assembly> _assemblies = new();

    public IEnumerable<Assembly> Assemblies => _assemblies.Distinct().ToArray();

    /// <summary>
    /// 
    /// </summary>
    public RegistrationOption(IServiceCollection services)
    {
        Services = services;
        RequestHandlerLifetime = ServiceLifetime.Scoped;
        MessageHandlerLifetime = ServiceLifetime.Scoped;
        StreamHandlerLifetime = ServiceLifetime.Scoped;
        
        RegisterAssembly(typeof(ISender).Assembly);
    }

    internal RegistrationOption(ServiceLifetime requestHandlerLifetime, ServiceLifetime messageHandlerLifetime, IServiceCollection services)
    {
        RequestHandlerLifetime = requestHandlerLifetime;
        MessageHandlerLifetime = messageHandlerLifetime;
        Services = services;
        StreamHandlerLifetime = messageHandlerLifetime;
        PublishStrategy = PublishStrategy.Sequential;
    }
    
    internal ServiceLifetime RequestHandlerLifetime { get; private set; }
    internal ServiceLifetime MessageHandlerLifetime { get; private set; }
    internal ServiceLifetime StreamHandlerLifetime { get; private set; }
    internal PublishStrategy PublishStrategy { get; private set; }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public void UsePublishStrategy(PublishStrategy strategy) => this.PublishStrategy = strategy;

    /// <inheritdoc />
    public void AsTransient()
    {

        RequestHandlerLifetime = ServiceLifetime.Transient;
        MessageHandlerLifetime = ServiceLifetime.Transient;
        StreamHandlerLifetime = ServiceLifetime.Transient;
    }

    /// <inheritdoc />
    public void AsScoped()
    {
        RequestHandlerLifetime = ServiceLifetime.Scoped;
        MessageHandlerLifetime = ServiceLifetime.Scoped;
        StreamHandlerLifetime = ServiceLifetime.Scoped;
    }

    /// <inheritdoc />
    public void AsSingleton()
    {
        RequestHandlerLifetime = ServiceLifetime.Singleton;
        MessageHandlerLifetime = ServiceLifetime.Singleton;
        StreamHandlerLifetime = ServiceLifetime.Singleton;
    }

    /// <inheritdoc />
    public void RegisterAssembly(Assembly assembly) => _assemblies.Add(assembly);
    
    /// <inheritdoc />
    public void RegisterAssemblies(IEnumerable<Assembly> assemblies) => _assemblies.AddRange(assemblies);
}