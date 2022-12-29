using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Segres.Abstractions;

namespace Segres;

public static class ServiceRegistration
{
    public static IServiceCollection AddSegres(this IServiceCollection services, Action<SegresConfiguration>? options = null)
        => services.AddSegres(Assembly.GetCallingAssembly(), null, options);
    
    public static IServiceCollection AddSegres(this IServiceCollection services, IConfiguration? configuration, Action<SegresConfiguration>? options = null)
        => services.AddSegres(Assembly.GetCallingAssembly(), configuration, options);

    private static IServiceCollection AddSegres(this IServiceCollection services, Assembly assembly, IConfiguration? configuration, Action<SegresConfiguration>? options = null)
    {
        var segresConfiguration = SegresConfiguration.Create(assembly, services, configuration, options);

        services.AddInstallerRegistrations(segresConfiguration);

        services.TryAddHandlers(typeof(IAsyncRequestHandler<>), segresConfiguration);
        services.TryAddHandlers(typeof(IAsyncRequestHandler<,>), segresConfiguration);
        // services.TryAddHandlers(typeof(IStreamRequestHandler<,>), segresConfiguration);
        services.TryAddHandlers(typeof(IAsyncNotificationHandler<>), segresConfiguration);

        services.TryAddSingleton(typeof(ISender), provider => new Sender(provider.CreateServiceResolver(segresConfiguration), new Dictionary<Type, object>()));
        // services.TryAddSingleton(typeof(IStreamer), provider => new Streamer(provider.CreateServiceResolver(segresConfiguration), new Dictionary<Type, object>()));
        services.TryAddSingleton(typeof(IPublisher), segresConfiguration.PublisherType);
        services.TryAddSingleton(typeof(ISubscriber), provider => new Subscriber(provider.CreateServiceResolver(segresConfiguration), segresConfiguration.PublisherStrategy));

        return services.AddSingleton(segresConfiguration);
    }

    private static void AddInstallerRegistrations(this IServiceCollection services, SegresConfiguration segresConfiguration)
    {
        var classTypes = segresConfiguration.Assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => IsAssignableToType(typeof(IServiceInstaller), t));

        var method = typeof(ServiceRegistration).GetMethod(nameof(InstallFromInstaller), BindingFlags.Static | BindingFlags.NonPublic)!;
        var parameters = new object?[] {services, segresConfiguration.Configuration};

        foreach (var classType in classTypes)
        {
            method.MakeGenericMethod(classType)
                .Invoke(null, parameters);
        }

        static bool IsAssignableToType(Type matchingType, TypeInfo typeInfo) =>
            matchingType.IsAssignableFrom(typeInfo) &&
            typeInfo is {IsInterface: false, IsAbstract: false};
    }

    private static void InstallFromInstaller<T>(IServiceCollection services, IConfiguration? configuration)
        where T : IServiceInstaller => T.Install(services, configuration);

    private static void TryAddHandlers(this IServiceCollection services, Type type, SegresConfiguration configuration)
    {
        var descriptors = configuration.Assemblies
            .GetGenericHandlers(type)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, configuration.ServiceLifetime));

        services.Add(descriptors);
    }

    private static Func<Type, object?> CreateServiceResolver(this IServiceProvider serviceProvider, SegresConfiguration configuration)
        => configuration.ServiceLifetime is ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider.GetService
            : serviceProvider.GetService;
}