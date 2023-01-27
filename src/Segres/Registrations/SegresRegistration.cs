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
public static partial class SegresRegistration
{
    private static readonly Type GenericRequestBehaviorType = typeof(IRequestBehavior<,>);
    private static readonly Type PublisherContextType = typeof(PublisherContext);


    /// <summary>
    /// Registers handlers and behaviors types from the calling assembly and all referenced assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="serviceLifetime">The lifetime of the handlers.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var assembly = Assembly.GetCallingAssembly();
        
        var assemblies = assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Append(assembly)
            .Append(typeof(SegresRegistration).Assembly)
            .ToHashSet();
        
        return services.AddSegres(serviceLifetime, assemblies);
    }

    /// <summary>
    /// Registers handlers and behaviors types from the assemblies of the specified makers.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="markers">The assembly makers to scan.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(this IServiceCollection services, params Type[] markers) 
        => services.AddSegres(ServiceLifetime.Scoped, markers);

    /// <summary>
    /// Registers handlers and behaviors types from the assemblies of the specified makers.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="serviceLifetime">The lifetime of the handlers.</param>
    /// <param name="markers">The assembly makers to scan.</param>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention AddSegres(
        this IServiceCollection services, 
        ServiceLifetime serviceLifetime,
        params Type[] markers)
    {
        var assemblies = markers
            .Select(x => x.Assembly)
            .Append(typeof(SegresRegistration).Assembly)
            .ToHashSet();

        return services.AddSegres(serviceLifetime, assemblies);
    }
    
    private static SegresConvention AddSegres(this IServiceCollection services, ServiceLifetime serviceLifetime, IReadOnlySet<Assembly> assemblies)
    {
        var segresConvention = new SegresConvention
        {
            Assemblies = assemblies,
            ServiceLifetime = serviceLifetime,
            Services = services
        };
        
        services.RegisterRequestHandlers(assemblies, serviceLifetime);
        services.RegisterRequestBehaviors(assemblies, serviceLifetime);
        services.RegisterEventHandlers(assemblies, serviceLifetime);

        services.AddSingleton(segresConvention);
        services.TryAddSingleton<ISender>(provider => provider.GetRequiredService<IMediator>());
        services.TryAddSingleton<IPublisher>(provider => provider.GetRequiredService<IMediator>());
        services.TryAddSingleton<IMediator>(p =>
        {
            var provider = p.CreateServiceResolver(serviceLifetime);
            return new Mediator(provider, new Dictionary<Type, object>());
        });
        
        segresConvention.ReplacePublisherContext<DefaultPublisherContext>();
        return segresConvention;
    }

    /// <summary>
    /// Register a custom <see cref="PublisherContext"/> to change the behavior publishing <see cref="IEvent"/>'s.
    /// </summary>
    /// <typeparam name="T"><see cref="PublisherContext"/></typeparam>
    /// <remarks>
    /// This is useful for implementing the Outbox-Pattern.
    /// </remarks>
    /// <returns><see cref="SegresConvention"/></returns>
    public static SegresConvention ReplacePublisherContext<T>(this SegresConvention segresConvention)
        where T : PublisherContext
    {
        var services = segresConvention.Services;

        for (var i = services.Count - 1; i >= 0; i--)
        {
            var serviceType = services[i].ServiceType;

            if (serviceType == PublisherContextType || serviceType.IsSubclassOf(PublisherContextType))
                services.RemoveAt(i);
        }

        services.TryAddSingleton<T>();
        services.TryAddSingleton(PublisherContextType, x => x.GetRequiredService<T>());

        return segresConvention;
    }

    private static void RegisterRequestHandlers(this IServiceCollection services, IReadOnlySet<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var handler1 = assemblies.GetGenericHandlers(typeof(IRequestHandler<>));
        var handler2 = assemblies.GetGenericHandlers(typeof(IRequestHandler<,>));


        var descriptors = handler1
            .Concat(handler2)
            .ToDictionary(x => x.Key, v => v.Value)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, serviceLifetime));

        services.Add(descriptors);
    }
    
    private static void RegisterRequestBehaviors(this IServiceCollection services, IReadOnlySet<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var typeToMatch = typeof(IRequestBehavior<,>);

        var descriptors = assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(t => t is { IsAbstract: false, IsInterface: false } && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == GenericRequestBehaviorType))
            .SelectMany(behaviorType => GetBehaviorInfos(behaviorType, typeToMatch))
            .ToHashSet()
            .Select(x => new ServiceDescriptor(x.Key, x.Value, serviceLifetime));

       services.Add(descriptors);
    }
    
    private static void RegisterEventHandlers(this IServiceCollection services, IReadOnlySet<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var typeToMatch = typeof(IEventHandler<>);

        var descriptors = assemblies
            .GetGenericHandlers(typeToMatch)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, serviceLifetime));

        services.Add(descriptors);
    }
    
    private static IEnumerable<KeyValuePair<Type, Type>> GetGenericHandlers(this IReadOnlySet<Assembly> assemblies, Type typeToMatch)
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
    
    private static bool IsInstance(this Type type)
        => type is {IsInterface: false, IsAbstract: false};
    
    private static KeyValuePair<Type, Type> GetTypeCombinations(KeyValuePair<Type, Type> keyValuePair)
    {
        if (!keyValuePair.Key.IsGenericType || !keyValuePair.Value.IsGenericType)
            return keyValuePair;

        var key = keyValuePair.Key.GetGenericTypeDefinition();
        var value = keyValuePair.Value.GetGenericTypeDefinition();

        return new KeyValuePair<Type, Type>(key, value);
    }

    private static IServiceProvider CreateServiceResolver(this IServiceProvider serviceProvider, ServiceLifetime serviceLifetime)
        => serviceLifetime is ServiceLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider
            : serviceProvider;
}