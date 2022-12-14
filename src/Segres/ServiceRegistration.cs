using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Segres.Communication;

namespace Segres;

/// <summary>
/// 
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSegres(this IServiceCollection services, Action<SegresConfiguration>? options = null)
    {
        var callingAssembly = Assembly.GetCallingAssembly();

        var assemblies = callingAssembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Append(callingAssembly)
            .Distinct()
            .ToList();

        var configuration = SegresConfiguration
            .Create()
            .RegisterAssemblies(assemblies);

        options?.Invoke(configuration);

        services.AddSender(configuration);
        services.AddPublisher(configuration);
        services.AddStreamer(configuration);

        services.AddSingleton(configuration);
        return services;
    }

    private static void AddStreamer(this IServiceCollection services, SegresConfiguration configuration)
    {
        services.TryAddHandlers(typeof(IStreamHandler<,>), configuration.Assemblies, configuration.Lifetime);
        services.TryAddSingleton<IStreamer>(provider => new Streamer(provider, configuration));
    }

    private static void AddPublisher(this IServiceCollection services, SegresConfiguration configuration)
    {
        services.TryAddMessageHandlers(typeof(INotificationHandler<>), configuration.Assemblies, configuration.Lifetime);
        services.TryAddSingleton(typeof(IPublisher), configuration.PublisherType);
        services.TryAddSingleton<ISubscriber>(provider => new Subscriber(provider, configuration));
    }

    private static void AddSender(this IServiceCollection services, SegresConfiguration configuration)
    {
        services.TryAddHandlers(typeof(IRequestHandler<>), configuration.Assemblies, configuration.Lifetime);
        services.TryAddHandlers(typeof(IRequestHandler<,>), configuration.Assemblies, configuration.Lifetime);
        services.TryAddSingleton<ISender>(provider => new Sender(provider, configuration));
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var implementationDescriptors = GetClassesImplementingInterface(assemblies, type).Distinct();
        services.Add(implementationDescriptors.SelectMany(x =>
        {
            return x.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                .Select(i => i.GetGenericArguments())
                .Select(i => new ServiceDescriptor(type.MakeGenericType(i), x, serviceLifetime));
        }));
    }

    private static void TryAddMessageHandlers(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        List<KeyValuePair<Type, Type>> descriptors = new();

        foreach (var assembly in assemblies)
        {
            var classes = assembly.GetHandlerTypeInfos(type);

            var pairs = classes.Select(implementationType =>
            {
                var argumentType = implementationType.GetInterfaces()
                    .FirstOrDefault(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == type)!
                    .GetGenericArguments();

                var interfaceType = type.MakeGenericType(argumentType);
                return new KeyValuePair<Type, Type>(interfaceType, implementationType);
            });

            descriptors.AddRange(pairs);
        }

        services.Add(descriptors.Distinct().Select(x => new ServiceDescriptor(x.Key, x.Value, serviceLifetime)));
    }

    private static IEnumerable<TypeInfo> GetHandlerTypeInfos(this Assembly assembly, Type type)
    {
        var classes = assembly.DefinedTypes.Where(t =>
        {
            var isImplementRequestType = t
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == type);

            return !t.IsInterface && !t.IsAbstract && isImplementRequestType;
        });
        return classes;
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(IEnumerable<Assembly> assemblies, Type typeToMatch)
    {
        List<Type> list = new();

        foreach (var assembly in assemblies)
        {
            var classes = GetClassesImplementingInterface(assembly, typeToMatch);
            list.AddRange(classes);
        }

        return list;
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

            return !type.IsInterface && !type.IsAbstract && isImplementRequestType;
        }).ToList();
    }
}