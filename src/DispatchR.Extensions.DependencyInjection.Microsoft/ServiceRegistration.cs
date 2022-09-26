using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DispatchR.Extensions.DependencyInjection.Microsoft;

/// <summary>
/// 
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TMarker"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarker>(this IServiceCollection services)
        => services.AddDispatchR(typeof(TMarker));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarkerOne, TMarkerTwo>(this IServiceCollection services)
        => services.AddDispatchR(typeof(TMarkerOne), typeof(TMarkerTwo));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <typeparam name="TMarkerThree"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarkerOne, TMarkerTwo, TMarkerThree>(this IServiceCollection services)
        => services.AddDispatchR(typeof(TMarkerOne), typeof(TMarkerTwo), typeof(TMarkerThree));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR(this IServiceCollection services, params Type[] types)
    {
        var assemblies = types.Select(x => x.Assembly).ToArray();
        return services.AddDispatchR(assemblies);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services.AddDispatchR(assemblies.AsSpan());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR(this IServiceCollection services, ReadOnlySpan<Assembly> assemblies)
    {
        const ServiceLifetime serviceLifetime = ServiceLifetime.Transient; // todo

        // HANDLERS
        services.TryAddHandlers(typeof(IQueryHandler<,>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<,>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(IMessageHandler<>), assemblies, serviceLifetime);

        var assemblyArray = assemblies.ToArray();

        // MEDIATORS
        services.TryAddSingleton<IDispatcher>(provider => new Dispatcher(provider, assemblyArray));
        services.TryAddSingleton<ISender>(x => x.GetRequiredService<IDispatcher>());

        services.BuildServiceProvider().GetService<IDispatcher>();

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