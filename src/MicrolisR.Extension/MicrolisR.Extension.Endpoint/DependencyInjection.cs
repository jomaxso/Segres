using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MicrolisR;

public static class DependencyInjection
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assemblies = new List<Assembly> {Assembly.GetCallingAssembly()};

        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly is not null)
            assemblies.Add(entryAssembly);

        var interfaceType = typeof(IHttpContextRequestResolver);

        foreach (var assembly in assemblies.Distinct())
        {
            var endpointResolvers = assembly.GetClassesImplementingInterface(interfaceType);

            foreach (var endpointResolver in endpointResolvers)
            {
                services.AddSingleton(interfaceType, endpointResolver);
                //services.Add(new ServiceDescriptor(interfaceType, endpointResolver, ServiceLifetime.Singleton));
            }
        }

        return services;
    }

    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointDetails = app.ServiceProvider
            .GetServices<IHttpContextRequestResolver>()
            .DistinctBy(x => x.GetType())
            .ToArray();

        foreach (var endpointDetail in endpointDetails)
        {
            switch (endpointDetail.HttpVerb)
            {
                case Http.GET:
                    app.MapGet(endpointDetail.Route, endpointDetail.EndpointDelegate);
                    continue;
                case Http.PUT:
                    app.MapPut(endpointDetail.Route, endpointDetail.EndpointDelegate);
                    continue;
                case Http.POST:
                    app.MapPost(endpointDetail.Route, endpointDetail.EndpointDelegate);
                    continue;
                case Http.DELETE:
                    app.MapDelete(endpointDetail.Route, endpointDetail.EndpointDelegate);
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }

    // public static void MapEndpoint<TRequest>(this IEndpointRouteBuilder app, Http verb, string pattern, bool validate = true)
    //     where TRequest : IRequestable
    // {
    //     app.MapEndpoint<TRequest, Unit>(verb, pattern, validate);
    // }
    
    private static IEnumerable<Type> GetClassesImplementingInterface(this Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Any(x => x.IsAssignableFrom(typeToMatch));

            return !type.IsInterface && !type.IsAbstract && isImplementRequestType;
        });
    }
}