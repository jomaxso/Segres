using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Segres.Abstractions;

namespace Segres;

/// <summary>
/// Extensions to scan and register for Segres handlers and behaviors.
/// - Scans for any <see cref="IAsyncRequestHandler{TRequest}"/> interface implementations and registers them as <see cref="ServiceLifetime.Scoped"/>.
/// - Scans for any <see cref="IAsyncRequestBehavior{TRequest, TResult}"/> implementations as well as the open generic implementations and registers them as <see cref="ServiceLifetime.Scoped"/>.
/// Registers <see cref="ISender"/> as <see cref="ServiceLifetime.Singleton"/>.
/// Registers <see cref="ISubscriber"/> as <see cref="ServiceLifetime.Singleton"/>.
/// Registers <see cref="IPublisher"/> as <see cref="ServiceLifetime.Singleton"/>.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers handlers and behaviors types from the calling assembly and all referenced assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="options">The action used to configure the options.</param>
    /// <returns></returns>
    public static ISegresContext AddSegres(this IServiceCollection services, Action<SegresConfiguration>? options = null)
    {
        var segresConfiguration = SegresConfiguration.Create(Assembly.GetCallingAssembly(), services, options);

        services.TryAddHandlers(segresConfiguration);
        services.TryAddBehaviors(segresConfiguration);

        services.TryAddSingleton(typeof(ISender), provider => new Sender(provider.CreateServiceResolver(segresConfiguration), new Dictionary<Type, object>()));
        services.TryAddSingleton(typeof(IPublisher), segresConfiguration.PublisherType);
        services.TryAddSingleton(typeof(ISubscriber), provider => new Subscriber(provider.CreateServiceResolver(segresConfiguration), segresConfiguration.PublisherStrategy));

        services.AddSingleton<ISegresContext>(segresConfiguration);

        return segresConfiguration;
    }

    private static void TryAddBehaviors(this IServiceCollection services, SegresConfiguration configuration)
    {
        var type = typeof(IAsyncRequestBehavior<,>);

        var descriptors = configuration.Assemblies
            .GetGenericHandlers(type)
            .Select(x =>
            {
                var (key, value) = GetTypeCombinations(x);
                return new ServiceDescriptor(key, value, configuration.ServiceLifetime);
            })
            .Distinct()
            .ToArray();

        services.Add(descriptors);
    }

    private static void TryAddHandlers(this IServiceCollection services, SegresConfiguration configuration)
    {
        services.TryAddHandlers(typeof(IAsyncRequestHandler<>), configuration);
        services.TryAddHandlers(typeof(IAsyncRequestHandler<,>), configuration);
        services.TryAddHandlers(typeof(IAsyncNotificationHandler<>), configuration);
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, SegresConfiguration configuration, Func<KeyValuePair<Type, Type>, bool>? condition = default)
    {
        var descriptors = configuration.Assemblies
            .GetGenericHandlers(type)
            .Where(x => condition?.Invoke(x) ?? true)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, configuration.ServiceLifetime))
            .ToArray();

        services.Add(descriptors);
    }

    private static Func<Type, object?> CreateServiceResolver(this IServiceProvider serviceProvider, SegresConfiguration configuration)
        => configuration.ServiceLifetime is ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider.GetService
            : serviceProvider.GetService;

    private static IEnumerable<KeyValuePair<Type, Type>> GetGenericHandlers(this IEnumerable<Assembly> assemblies, Type type)
    {
        return assemblies.GetClassesImplementingInterface(type)
            .Distinct()
            .SelectMany(implementationType =>
            {
                return implementationType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                    .Select(i => i.GetGenericArguments())
                    .Select(i => new KeyValuePair<Type, Type>(type.MakeGenericType(i), implementationType));
            })
            .Distinct();
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(this IEnumerable<Assembly> assemblies, Type typeToMatch)
        => assemblies.SelectMany(assembly => GetClassesImplementingInterface(assembly, typeToMatch));

    private static IEnumerable<Type> GetClassesImplementingInterface(this Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

            return type.IsInstance() && isImplementRequestType;
        });
    }
    
    private static KeyValuePair<Type, Type> GetTypeCombinations(KeyValuePair<Type, Type> keyValuePair)
    {
        if (!keyValuePair.Key.IsGenericType || !keyValuePair.Value.IsGenericType) 
            return keyValuePair;
        
        var key = keyValuePair.Key.GetGenericTypeDefinition();
        var value = keyValuePair.Value.GetGenericTypeDefinition();

        return new KeyValuePair<Type, Type>(key, value);
    }

    private static bool IsInstance(this Type type)
        => type is {IsInterface: false, IsAbstract: false};

    internal static IEnumerable<Assembly> AppendReferencedAssemblies(this Assembly assembly)
    {
        return assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Append(assembly)
            .Distinct();
    }
}