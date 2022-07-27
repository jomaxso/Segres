using System.Reflection;
using MicrolisR;
using PrintToConsole;

namespace Demo;

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
                services.Add(new ServiceDescriptor(interfaceType, endpointResolver, ServiceLifetime.Singleton));
            }
        }

        return services;
    }

    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var httpContextRequestResolvers = app.ServiceProvider
            .GetServices<IHttpContextRequestResolver>()
            .DistinctBy(x => x.GetType());

        foreach (var httpContextRequestResolver in httpContextRequestResolvers)
            httpContextRequestResolver.Initialize(app);
    }
    
    public static void MapEndpoint<TRequest, TResponse>(this IEndpointRouteBuilder app, Http verb, string pattern)
        where TRequest : IRequestable<TResponse>
    {
        string[] verbs = {verb.ToString()};

        var mediator = app.ServiceProvider.GetRequiredService<IMediator>();
        var httpContextRequestResolver = app.ServiceProvider.GetRequiredService<IHttpContextRequestResolver>();

        app.MapMethods(pattern, verbs, async context =>
        {
            var request = await httpContextRequestResolver.ExtractRequest<TRequest>(context.Request, context.RequestAborted);

            mediator.Validate(request);

            if (request is IRequestable withoutResponse)
            {
                await mediator.SendAsync(withoutResponse, context.RequestAborted);
                await context.Response.CompleteAsync();
                return;
            }

            var response = await mediator.SendAsync(request, context.RequestAborted);

            if (response is IValidatable validatable)
                mediator.Validate(validatable);

            await context.Response.WriteAsJsonAsync(response);
            await context.Response.CompleteAsync();
        });
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(this Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Any(x => x.IsAssignableFrom(typeToMatch));

            return !type.IsInterface && !type.IsAbstract && isImplementRequestType;
        }).ToList();
    }
}

public interface IHttpContextRequestResolver
{
    internal void Initialize(IEndpointRouteBuilder app);

    Task<TRequest> ExtractRequest<TRequest>(HttpRequest httpRequest, CancellationToken cancellationToken);
}

public interface IHttpContextRequestResolver<TRequest, TResponse> : IHttpContextRequestResolver
    where TRequest : IRequestable<TResponse>
{
    void IHttpContextRequestResolver.Initialize(IEndpointRouteBuilder app)
    {
        var handler = app.ServiceProvider.GetRequiredService(typeof(IRequestHandler<TRequest, TResponse>)); // BUG not found in DI
        
        // TODO REfelction get all Attribute infos

        var verb = Http.GET;
        var pattern = "/";
        
        app.MapEndpoint<TRequest, TResponse>(verb, pattern);
    }
}


/// <summary>
/// TO GENERATE
/// </summary>
public class HttpContextRequestResolver : IHttpContextRequestResolver<PrintRequest, Unit> //  Generate Source
{
    public async Task<TRequest> ExtractRequest<TRequest>(HttpRequest httpRequest, CancellationToken cancellationToken)
    {
        var request = await httpRequest.ReadFromJsonAsync<TRequest>(cancellationToken);

        if (request is null)
            throw new Exception();

        return request;
    }
}