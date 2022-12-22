using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    public static IServiceCollection AddSegres(this IServiceCollection services, Action<SegresConfiguration>? options = null)
    {
        var configuration = SegresConfiguration.Create(Assembly.GetCallingAssembly(), options);

        services.TryAddHandlers(typeof(IAsyncRequestHandler<>), configuration);
        services.TryAddHandlers(typeof(IAsyncRequestHandler<,>), configuration);
        services.TryAddHandlers(typeof(IStreamHandler<,>), configuration);
        services.TryAddHandlers(typeof(INotificationHandler<>), configuration);

        services.TryAddSingleton(typeof(ISender), provider => new Sender(provider.CreateServiceResolver(configuration)));
        services.TryAddSingleton(typeof(IStreamer), provider => new Streamer(provider.CreateServiceResolver(configuration)));
        services.TryAddSingleton(typeof(IPublisher), configuration.PublisherType);
        services.TryAddSingleton(typeof(ISubscriber), provider => new Subscriber(provider.CreateServiceResolver(configuration), configuration.PublisherStrategy));

        return services.AddSingleton(configuration);
    }

    private static void TryAddHandlers(this IServiceCollection services, Type type, SegresConfiguration configuration)
    {
        var serviceLifetime = configuration.Lifetime.GetServiceLifetime();

        var descriptors = configuration.Assemblies
            .GetGenericHandlers(type)
            .Select(x => new ServiceDescriptor(x.Key, x.Value, serviceLifetime));

        services.Add(descriptors);
    }
    
    private static ServiceResolver CreateServiceResolver(this IServiceProvider serviceProvider, SegresConfiguration configuration)
        => configuration.Lifetime is HandlerLifetime.Scoped
            ? serviceProvider.CreateScope().ServiceProvider.GetService
            : serviceProvider.GetService;

    private static ServiceLifetime GetServiceLifetime(this HandlerLifetime handlerLifetime)
        => handlerLifetime switch
        {
            HandlerLifetime.Singleton => ServiceLifetime.Singleton,
            HandlerLifetime.Scoped => ServiceLifetime.Scoped,
            HandlerLifetime.Transient => ServiceLifetime.Transient,
            _ => throw new UnreachableException()
        };
}