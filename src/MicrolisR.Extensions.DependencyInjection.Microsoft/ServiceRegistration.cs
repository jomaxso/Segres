using System.Reflection;
using MicrolisR.Abstractions;
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
        var serviceLifetime = ServiceLifetime.Singleton; // todo

        // HANDLERS
        services.TryAddHandlers(typeof(IReceiver<,>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(IValidation<>), assemblies, serviceLifetime);
        services.TryAddHandlers(typeof(ISubscriber<>), assemblies, serviceLifetime);

        services.TryAddSingleton<IValidator>(provider => new Validator(provider, assemblies));
        
        // MEDIATORS
        services.TryAddSingleton<IMediator>(provider => new Mediator(provider, assemblies));
        services.TryAddSingleton<ISender>(x => x.GetRequiredService<IMediator>());
        services.TryAddSingleton<IPublisher>(x => x.GetRequiredService<IMediator>());


        return services;
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
    {
        var descriptors = GetClassesImplementingInterface(assemblies, type);
        services.TryAdd(descriptors.Select(x => new ServiceDescriptor(x, x, serviceLifetime)));
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