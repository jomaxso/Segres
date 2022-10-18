using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Segres.Handlers;

namespace Segres.Extensions.DependencyInjection.Microsoft;

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
        => services.AddSegres(Assembly.GetCallingAssembly()!.GetExportedTypes(), options);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarker"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddSegres<TMarker>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddSegres(new[] {typeof(TMarker)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddSegres<TMarkerOne, TMarkerTwo>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddSegres(new[] {typeof(TMarkerOne), typeof(TMarkerTwo)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <typeparam name="TMarkerThree"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddSegres<TMarkerOne, TMarkerTwo, TMarkerThree>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddSegres(new[] {typeof(TMarkerOne), typeof(TMarkerTwo), typeof(TMarkerThree)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="markers"></param>
    /// <returns></returns>
    public static IServiceCollection AddSegres(this IServiceCollection services, params Type[] markers) 
        => services.AddSegres(markers, null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="markers"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddSegres(this IServiceCollection services, IEnumerable<Type> markers, Action<RegistrationOption>? options = null)
    {
        var types = markers.Distinct().ToArray();

        var registrationOption = new RegistrationOption();
        options?.Invoke(registrationOption);

        var assemblies = types.Select(x => x.Assembly).Distinct().ToArray();
        
        // HANDLERS
        services.TryAddHandlers(typeof(IQueryHandler<,>), assemblies, registrationOption.QueryHandlerLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<>), assemblies, registrationOption.CommandHandlerLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<,>), assemblies, registrationOption.CommandHandlerLifetime);
        services.TryAddHandlers(typeof(IMessageHandler<>), assemblies, registrationOption.MessageHandlerLifetime);
        services.TryAddHandlers(typeof(IStreamHandler<,>), assemblies, registrationOption.StreamHandlerLifetime);
        
        // CORE SERVICES
        services.TryAddSingleton<IServiceBroker>(provider => new ServiceBroker(provider.GetRequiredService, types));
        services.TryAddSingleton<ISender>(provider => provider.GetRequiredService<IServiceBroker>());
        services.TryAddSingleton<IPublisher>(provider => provider.GetRequiredService<IServiceBroker>());
        services.TryAddSingleton<IStreamer>(provider => provider.GetRequiredService<IServiceBroker>());
        
        return services;
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, ReadOnlySpan<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var descriptors = GetClassesImplementingInterface(assemblies, type).Distinct();
        services.TryAdd(descriptors.Select(x => new ServiceDescriptor(x, x, serviceLifetime)));
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(ReadOnlySpan<Assembly> assemblies, Type typeToMatch)
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