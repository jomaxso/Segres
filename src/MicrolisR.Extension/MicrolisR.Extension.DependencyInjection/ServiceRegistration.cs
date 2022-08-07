using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MicrolisR.Extensions.Microsoft.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddMicrolisR(this IServiceCollection services, params Type[] types)
    {
        var assemblies = types.Select(x => x.Assembly).ToArray();
        return services.AddMicrolisR(assemblies);
    }

    public static IServiceCollection AddMicrolisR(this IServiceCollection services, params Assembly[] assemblies)
    {
        // REQUEST_HANDLER
        var handlerDetails = GetHandlerDetails(assemblies, typeof(IRequestHandler<,>))
            .ToDictionary(x => x.Key, y => y.Value);

        foreach (var implementationType in handlerDetails.Values)
        {
            var serviceDescriptors = implementationType
                .GetInterfaces()
                .Where(x => x.Name == (typeof(IRequestHandler<,>).Name))
                .Select(x => new ServiceDescriptor(x, implementationType, ServiceLifetime.Scoped));
            
            services.TryAdd(serviceDescriptors);
            services.TryAdd(new ServiceDescriptor(implementationType, implementationType, ServiceLifetime.Scoped));
        }

        services.TryAddSingleton<ISender>(x => new Sender(x.GetRequiredService, handlerDetails));

        // VALIDATION_HANDLER
        var validatorDetailsTypes = GetHandlerDetails(assemblies, typeof(IValidationHandler<>)).ToArray();
        services.TryAdd(validatorDetailsTypes.Select(x => new ServiceDescriptor(x.Value, x.Value, ServiceLifetime.Singleton)));
        services.TryAddSingleton<IValidator>(provider =>
        {
            var validatorDetails = validatorDetailsTypes
                .Select(x =>
                {
                    var handler = provider.GetRequiredService(x.Value);
                    return new KeyValuePair<Type, object>(x.Key, handler!);
                });

            return new Validator(validatorDetails);
        });

        // MAP_HANDLER
        var mapperDetails = GetHandlerDetails(assemblies, typeof(IMapHandler<,>)).ToDictionary(x => x.Key, y => y.Value);
        services.TryAdd(mapperDetails.Values.Select(x => new ServiceDescriptor(x, x, ServiceLifetime.Singleton)));
        services.TryAddSingleton<IMapper>(x => new Mapper(x.GetRequiredService, mapperDetails));
        
        // MESSAGE_HANDLER
        var messageDetailsTypes = GetHandlerDetails(assemblies, typeof(IMessageHandler<>)).ToArray();
        services.TryAdd(messageDetailsTypes.Select(x => new ServiceDescriptor(x.Value, x.Value, ServiceLifetime.Scoped)));
        
        var details = new Dictionary<Type, List<Type>>();
        
        foreach (var handlerDetail in GetHandlerDetails(assemblies, typeof(IMessageHandler<>)))
        {
            if (details.ContainsKey(handlerDetail.Key))
            {
                details[handlerDetail.Key].Add(handlerDetail.Value);
                continue;
            }
            
            details.Add(handlerDetail.Key, new List<Type>(){ handlerDetail.Value });
        }

        services.TryAddSingleton<IPublisher>(x => new Publisher(x.GetService, details.ToDictionary(k => k.Key, v => v.Value.ToArray())));

        // MEDIATOR
        services.TryAddSingleton<IMediator, Mediator>();

        return services;
    }

    private static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(IEnumerable<Assembly> assemblies, Type type)
    {
        var handlerDetails = new List<KeyValuePair<Type, Type>>();

        foreach (var assembly in assemblies)
        {
            var handlers = GetHandlerDetails(assembly, type);

            foreach (var handler in handlers)
            {
                handlerDetails.Add(new KeyValuePair<Type, Type>(handler.Key, handler.Value));
            }
        }

        return handlerDetails;
    }

    private static List<KeyValuePair<Type, Type>> GetHandlerDetails(Assembly assembly, Type type)
    {
        var validators = GetClassesImplementingInterface(assembly, type);

        var details = new List<KeyValuePair<Type, Type>>();

        foreach (var validator in validators)
        {
            var requestTypes = validator
                .GetInterfaces()
                .Where(x => x.Name == type.Name)
                .Select(x => x.GetGenericArguments()[0]);

            foreach (var requestType in requestTypes)
            {
                details.Add(new KeyValuePair<Type, Type>(requestType, validator));
            }
        }

        return details;
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
    {
        return assembly.ExportedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

            return !type.IsInterface && !type.IsAbstract && isImplementRequestType;
        }).ToList();
    }
}