using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Segres.Tmp.Http;

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
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddSegres(this IServiceCollection services, Action<RegistrationOption>? options = null)
    {
        var registrationOption = new RegistrationOption(services);
        
        var assemblies = Assembly
            .GetCallingAssembly()!
            .GetExportedTypes()
            .Select(x => x.Assembly)
            .Distinct()
            .ToList();

        registrationOption.RegisterAssemblies(assemblies);

        options?.Invoke(registrationOption);

        services.AddSender(registrationOption);
        services.AddPublisher(registrationOption);
        services.AddStreamer(registrationOption);

        return services;
    }

    private static void AddStreamer(this IServiceCollection services, RegistrationOption options)
    {
        services.TryAddHandlers(typeof(IStreamHandler<,>), options.Assemblies, options.StreamHandlerLifetime);
        services.TryAddSingleton<IStreamer>(provider => new Streamer(provider));
    }
    
    private static void AddPublisher(this IServiceCollection services, RegistrationOption options)
    {
        services.TryAddMessageHandlers(typeof(INotificationHandler<>), options.Assemblies, options.MessageHandlerLifetime);
        services.TryAddSingleton<IPublisher>(provider => new Publisher(provider, options.PublishStrategy));
    }

    private static void AddSender(this IServiceCollection services, RegistrationOption options)
    {
        services.AddSingleton(typeof(HttpRequestHandler<,>));
        services.TryAddHandlers(typeof(IRequestHandler<>), options.Assemblies, options.RequestHandlerLifetime);
        services.TryAddHandlers(typeof(IRequestHandler<,>), options.Assemblies, options.RequestHandlerLifetime);
        services.TryAddSingleton<ISender>(provider => new Sender(options.RequestHandlerLifetime is ServiceLifetime.Scoped ? provider.CreateScope().ServiceProvider : provider));
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var implementationDescriptors = GetClassesImplementingInterface(assemblies, type).Distinct();
        services.Add(implementationDescriptors.SelectMany(x =>
        {
            return x.GetInterfaces()
                .Where(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == type)
                .Select(xx => xx.GetGenericArguments())
                .Select(xx => new ServiceDescriptor(type.MakeGenericType(xx), x, serviceLifetime));
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
                    .FirstOrDefault(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == typeof(INotificationHandler<>))!
                    .GetGenericArguments();

                var interfaceType = typeof(INotificationHandler<>).MakeGenericType(argumentType);
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