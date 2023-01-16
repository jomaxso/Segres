using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Segres;

/// <summary>
/// Extensions to scan and register for Segres handlers and behaviors.
/// - Scans for any <see cref="IRequestHandler{TRequest}"/> interface implementations and registers them as <see cref="ServiceLifetime.Scoped"/>.
/// - Scans for any <see cref="IRequestBehavior{TRequest,TResult}"/> implementations as well as the open generic implementations and registers them as <see cref="ServiceLifetime.Scoped"/>.
/// Registers <see cref="ISender"/> as <see cref="ServiceLifetime.Singleton"/>.
/// Registers <see cref="PublisherContext"/> with specified the lifetime.
/// Registers <see cref="IPublisher"/> as <see cref="ServiceLifetime.Singleton"/>.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers handlers and behaviors types from the calling assembly and all referenced assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();
        return services.AddSegres(x => x.UseReferencedAssemblies(assembly));
    }

    /// <summary>
    /// Registers handlers and behaviors types manual.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="options">The action used to configure the options.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(this IServiceCollection services, Action<SegresConventionBuilder> options)
        => services.AddSegres(new SegresConventionBuilder(services), options);

    /// <summary>
    /// Registers handlers and behaviors types manual.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="segresConventionBuilder">The convention builder.</param>
    /// <param name="options">The action used to configure the options.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(this IServiceCollection services, SegresConventionBuilder segresConventionBuilder, Action<SegresConventionBuilder>? options = null)
    {
        var segresConfiguration = segresConventionBuilder
            .UseAssembly(typeof(SegresConventionBuilder))
            .Build(options);

        services.AddSingleton(segresConfiguration);

        segresConfiguration
            .RegisterRequestHandlers()
            .RegisterNotificationHandlers()
            .RegisterBehaviors()
            .RegisterMediators();

        return segresConfiguration;
    }

    private static SegresConvention RegisterMediators(this SegresConvention configuration)
    {
        configuration.Services.TryAddSingleton(configuration.PublisherType);
        configuration.Services.TryAddSingleton(typeof(PublisherContext), x => x.GetRequiredService(configuration.PublisherType));

        configuration.Services.TryAddSingleton<IMediator>(p =>
        {
            var provider = p.CreateServiceResolver(configuration.ServiceLifetime);
            return new Mediator(provider, new Dictionary<Type, object>());
        });

        configuration.Services.TryAddSingleton<ISender>(provider => provider.GetRequiredService<IMediator>());
        configuration.Services.TryAddSingleton<IPublisher>(provider => provider.GetRequiredService<IMediator>());

        return configuration;
    }

    private static SegresConvention RegisterBehaviors(this SegresConvention configuration)
    {
        var typeToMatch = typeof(IRequestBehavior<,>);

        var descriptors = configuration.BehaviorTypes
            .Distinct()
            .SelectMany(behaviorType => GetBehaviorInfos(behaviorType, typeToMatch))
            .Distinct()
            .Select(x => new ServiceDescriptor(x.Key, x.Value, configuration.ServiceLifetime));

        configuration.Services.Add(descriptors);

        return configuration;
    }

    private static IEnumerable<KeyValuePair<Type, Type>> GetBehaviorInfos(Type behaviorType, Type typeToMatch)
    {
        var isImplementRequestType = behaviorType
            .GetInterfaces()
            .Where(x => x.IsGenericType)
            .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

        if (!behaviorType.IsInstance() || !isImplementRequestType)
            return Enumerable.Empty<KeyValuePair<Type, Type>>();

        return behaviorType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeToMatch)
            .Select(i => i.GetGenericArguments())
            .Select(i => new KeyValuePair<Type, Type>(typeToMatch.MakeGenericType(i), behaviorType))
            .Select(GetTypeCombinations);
    }

    private static SegresConvention RegisterNotificationHandlers(this SegresConvention configuration)
    {
        var typeToMatch = typeof(INotificationHandler<>);

        var descriptors = configuration.Assemblies
            .GetGenericHandlers(typeToMatch)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, configuration.ServiceLifetime));

        configuration.Services.Add(descriptors);

        return configuration;
    }

    private static SegresConvention RegisterRequestHandlers(this SegresConvention configuration)
    {
        var handler1 = configuration.Assemblies.GetGenericHandlers(typeof(IRequestHandler<>));
        var handler2 = configuration.Assemblies.GetGenericHandlers(typeof(IRequestHandler<,>));


        var descriptors = handler1
            .Concat(handler2)
            .ToDictionary(x => x.Key, v => v.Value)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, configuration.ServiceLifetime));

        configuration.Services.Add(descriptors);

        return configuration;
    }

    private static IServiceProvider CreateServiceResolver(this IServiceProvider serviceProvider, ServiceLifetime serviceLifetime)
        => serviceLifetime is ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider
            : serviceProvider;

    private static IEnumerable<KeyValuePair<Type, Type>> GetGenericHandlers(this IEnumerable<Assembly> assemblies, Type typeToMatch)
    {
        return assemblies
            .SelectMany(assembly => GetClassesImplementingInterface(assembly, typeToMatch))
            .SelectMany(implementationType =>
            {
                return implementationType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeToMatch)
                    .Select(i => i.GetGenericArguments())
                    .Select(i => new KeyValuePair<Type, Type>(typeToMatch.MakeGenericType(i), implementationType));
            });
    }

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
}
