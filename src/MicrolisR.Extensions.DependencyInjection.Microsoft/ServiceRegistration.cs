using System.Reflection;
using MicrolisR.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MicrolisR.Extensions.Microsoft.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddMicrolisR<TMarker>(this IServiceCollection services)
        => services.AddMicrolisR(typeof(TMarker));

    public static IServiceCollection AddMicrolisR<TMarkerOne, TMarkerTwo>(this IServiceCollection services)
        => services.AddMicrolisR(typeof(TMarkerOne), typeof(TMarkerTwo));

    public static IServiceCollection AddMicrolisR<TMarkerOne, TMarkerTwo, TMarkerThree>(this IServiceCollection services)
        => services.AddMicrolisR(typeof(TMarkerOne), typeof(TMarkerTwo), typeof(TMarkerThree));


    public static IServiceCollection AddMicrolisR(this IServiceCollection services, params Type[] types)
    {
        var assemblies = types.Select(x => x.Assembly).ToArray();
        return services.AddMicrolisR(assemblies);
    }


    public static IServiceCollection AddMicrolisR(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services.AddMicrolisR(assemblies.AsSpan());
    }
    
    public static IServiceCollection AddMicrolisR(this IServiceCollection services, ReadOnlySpan<Assembly> assemblies)
    {
        const ServiceLifetime serviceLifetime = ServiceLifetime.Transient; // todo

        // HANDLERS
        services.TryAddHandlers(typeof(IQueryHandler<,>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(ICommandHandler<,>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(IValidation<>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(INotificationHandler<>), assemblies, serviceLifetime);

        var assemblyArray = assemblies.ToArray();
        services.TryAddSingleton<IValidator>(provider => new Validator(provider, assemblyArray));
        
        // MEDIATORS
        services.TryAddSingleton<IDispatcher>(provider => new Dispatcher(provider, assemblyArray));
        services.TryAddSingleton<IQuerySender>(x => x.GetRequiredService<IDispatcher>());
        services.TryAddSingleton<ICommandSender>(x => x.GetRequiredService<IDispatcher>());
        services.TryAddSingleton<IPublisher>(x => x.GetRequiredService<IDispatcher>());

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