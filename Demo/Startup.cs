using System.Reflection;
using Demo.Endpoints.PrintToConsole;
using MicrolisR;

namespace Demo;

public static class Startup
{
    private static void MapMicrolisR(this IEndpointRouteBuilder app)
    {
        // app.MapPost("/", (IMediator mediator, [FromBody] PrintCommand command, CancellationToken cancellationToken) => mediator.SendAsync(command, cancellationToken));
        //
        // app.MapPost("/{value:int}", ([FromServices] IMediator mediator, [FromRoute] int value, [FromHeader] int value2, CancellationToken cancellationToken) => mediator.SendAsync(new PrintCommand {Value = value, Value2 = value2}, cancellationToken));
        //
        // app.MapPost("/", (IMediator mediator, [FromQuery] int value, CancellationToken cancellationToken) => mediator.SendAsync(new PrintCommand {Value = value}, cancellationToken));
        //
        // app.MapPost("/", (IMediator mediator, [FromHeader] int value, CancellationToken cancellationToken) => mediator.SendAsync(new PrintCommand {Value = value}, cancellationToken));
        //


        // app.MapGet("/", async (IMediator mediator, [FromQuery] int value, CancellationToken cancellationToken) =>
        // {
        //     var request = new PrintCommand
        //     {
        //         Value = value
        //     };
        //     
        //     return await mediator.SendAsync(request, cancellationToken);
        // });
        //
        // app.MapGet("/{value:int}", async (IMediator mediator, [FromRoute] int value, CancellationToken cancellationToken) => await mediator.SendAsync(new PrintCommand {Value = value}, cancellationToken));
        //
        // app.MapGet("/", async (IMediator mediator, [FromHeader] int value, CancellationToken cancellationToken) => await mediator.SendAsync(new PrintCommand {Value = value}, cancellationToken));
    }

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
                services.Add(new ServiceDescriptor(interfaceType,endpointResolver, ServiceLifetime.Singleton));
            }
        }
        
        return services;
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
    
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var httpContextRequestResolvers = app.ServiceProvider
            .GetServices<IHttpContextRequestResolver>()
            .DistinctBy(x => x.GetType());
        
        foreach (var httpContextRequestResolver in httpContextRequestResolvers)
            httpContextRequestResolver.MapEndpoints(app);
    }
    
    public static void MapEndpoint<TRequest, TResponse>(this IEndpointRouteBuilder app, Http verb, string pattern, IRequestHandler<TRequest, TResponse>? _)
        where TRequest : IRequestable<TResponse>
    {
        string[] verbs = { verb.ToString() };
        
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
}

public interface IHttpContextRequestResolver
{
    void MapEndpoints(IEndpointRouteBuilder app);

    Task<TRequest> ExtractRequest<TRequest>(HttpRequest httpRequest, CancellationToken cancellationToken);
}


/// <summary>
/// TO GENERATE
/// </summary>

public class HttpContextRequestResolver : IHttpContextRequestResolver //  Generate Source
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapEndpoint(Http.GET, "/", app.ServiceProvider.GetService<PrintEndpoint>());
    }
    
    public async Task<TRequest> ExtractRequest<TRequest>(HttpRequest httpRequest, CancellationToken cancellationToken)
    {
         var request =  await httpRequest.ReadFromJsonAsync<TRequest>(cancellationToken);

        if (request is null)
            throw new Exception();

        return request;
    }
}

