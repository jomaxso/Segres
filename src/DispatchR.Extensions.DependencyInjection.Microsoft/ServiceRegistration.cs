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
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddDispatchR(Assembly.GetCallingAssembly()!.GetExportedTypes(), options);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarker"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarker>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddDispatchR(new[] {typeof(TMarker)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarkerOne, TMarkerTwo>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddDispatchR(new[] {typeof(TMarkerOne), typeof(TMarkerTwo)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <typeparam name="TMarkerOne"></typeparam>
    /// <typeparam name="TMarkerTwo"></typeparam>
    /// <typeparam name="TMarkerThree"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR<TMarkerOne, TMarkerTwo, TMarkerThree>(this IServiceCollection services, Action<RegistrationOption>? options = null)
        => services.AddDispatchR(new[] {typeof(TMarkerOne), typeof(TMarkerTwo), typeof(TMarkerThree)}, options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="markers"></param>
    /// <returns></returns>
    public static IServiceCollection AddDispatchR(this IServiceCollection services, params Type[] markers) 
        => services.AddDispatchR(markers, null);

    public static IServiceCollection AddDispatchR(this IServiceCollection services, IEnumerable<Type> markers, Action<RegistrationOption>? options = null)
    {
        var types = markers.Distinct().ToArray();
        var assemblies = types.Select(x => x.Assembly).Distinct().ToArray();
        
        var registrationOption = new RegistrationOption();
        options?.Invoke(registrationOption);

        // HANDLERS
        services.TryAddHandlers(typeof(IQueryHandler<,>), assemblies, registrationOption.QueryHandlerLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<>), assemblies, registrationOption.CommandHandlerLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<,>), assemblies, registrationOption.CommandHandlerLifetime);
        services.TryAddHandlers(typeof(IMessageHandler<>), assemblies, registrationOption.MessageHandlerLifetime);
        services.TryAddHandlers(typeof(IStreamHandler<,>), assemblies, registrationOption.StreamHandlerLifetime);


        // MEDIATORS
        services.TryAddSingleton<IDispatcher>(provider => new Dispatcher(provider.GetRequiredService, types));
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