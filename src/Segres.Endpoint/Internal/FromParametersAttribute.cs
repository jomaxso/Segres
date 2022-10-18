using Microsoft.AspNetCore.Mvc;

namespace Segres.Endpoint;

public static partial class Registration
{
    // private static readonly MethodInfo MapEndpointMethod = typeof(Registration).GetMethod(nameof(MapEndpoint)) ?? throw new AmbiguousImplementationException();
    //
    // public static void MapEndpoints(this IEndpointRouteBuilder builder, Assembly assembly)
    // {
    //     var loggerFactory = builder.ServiceProvider.GetService<ILoggerFactory>();
    //     var logger = loggerFactory?.CreateLogger("Segres.Endpoints");
    //
    //     var requestTypes = assembly.GetRequestTypes(logger);
    //
    //     foreach (var (requestType, endpointType, endpointAttribute) in requestTypes)
    //     {
    //         builder.InternalMapEndpoint(requestType, endpointType, endpointAttribute.Route)
    //             .WithTags(endpointAttribute.Group ?? endpointAttribute.Route.Split('/')[0]);
    //     }
    // }
    //
    // public static RouteHandlerBuilder MapEndpoint<TRequest, TEndpoint>(this IEndpointRouteBuilder builder, string pattern)
    //     where TEndpoint : IEndpoint<TRequest>
    // {
    //     var interfaces = typeof(TRequest).GetInterfaces();
    //
    //     var isGetRequest = interfaces.Any(x => x == typeof(IHttpGetRequest));
    //     var isPostRequest = interfaces.Any(x => x == typeof(IHttpPostRequest));
    //     var isPutRequest = interfaces.Any(x => x == typeof(IHttpPutRequest));
    //     var isDeleteRequest = interfaces.Any(x => x == typeof(IHttpDeleteRequest));
    //
    //     if (isGetRequest)
    //         return builder.MapGet(pattern, EndpointDelegate<TRequest, TEndpoint>);
    //
    //     if (isPostRequest)
    //         return builder.MapPost(pattern, EndpointDelegate<TRequest, TEndpoint>);
    //
    //     if (isPutRequest)
    //         return builder.MapPut(pattern, EndpointDelegate<TRequest, TEndpoint>);
    //
    //     if (isDeleteRequest)
    //         return builder.MapDelete(pattern, EndpointDelegate<TRequest, TEndpoint>);
    //
    //     throw new ArgumentOutOfRangeException();
    // }
    //
    //
    // private static IEnumerable<ValueTuple<Type, Type, EndpointAttribute>> GetRequestTypes(this Assembly assembly, ILogger? logger)
    // {
    //     var endpointTypes = assembly.DefinedTypes.SelectMany(x =>
    //         {
    //             var interfaces = x.GetInterfaces();
    //             var counts = interfaces.Count(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEndpoint<>));
    //
    //             return Enumerable.Range(0, counts).Select(y => x);
    //         })
    //         .ToArray();
    //
    //     var registrationTuple = endpointTypes
    //         .Where(classType => classType.IsDefined(typeof(EndpointAttribute)))
    //         .SelectMany(x => x.GetInterfaces())
    //         .Where(x => x.IsGenericType)
    //         .Where(x => x.GetGenericTypeDefinition() == typeof(IEndpoint<>))
    //         .SelectMany(endpointType => endpointType.GetInterfaces().Select(interfaceType => (endpointType, interfaceType, endpointType.GetCustomAttribute<EndpointAttribute>()!)))
    //         .Select(x => (x.interfaceType.GetGenericArguments()[0], x.endpointType, x.Item3))
    //         .ToArray();
    //
    //     if (endpointTypes.Length > registrationTuple.Length)
    //     {
    //         foreach (var endpointType in endpointTypes)
    //         {
    //             if (registrationTuple.Any(x => x.endpointType == endpointType))
    //                 continue;
    //
    //             logger?.LogWarning("An endpoint of type {Endpoint} is not registered. Use the [EndpointAttribute] to register", endpointType.Name);
    //         }
    //     }
    //
    //     return registrationTuple;
    // }
    //
    // private static RouteHandlerBuilder InternalMapEndpoint(this IEndpointRouteBuilder builder, Type requestType, Type endpointType, string pattern)
    // {
    //     var mapEndpointDelegate = MapEndpointMethod
    //         .MakeGenericMethod(requestType, endpointType)
    //         .CreateDelegate<Func<IEndpointRouteBuilder, string, RouteHandlerBuilder>>();
    //
    //     return mapEndpointDelegate.Invoke(builder, pattern);
    // }
    //
    // private static Task<IResult> EndpointDelegate<TRequest, TEndpoint>([FromServices] IServiceProvider serviceProvider, [FromParameters] TRequest request, CancellationToken cancellationToken)
    //     where TEndpoint : IEndpoint<TRequest>
    // {
    //     var scope = serviceProvider.CreateScope();
    //     var endpoint = ActivatorUtilities.GetServiceOrCreateInstance<TEndpoint>(scope.ServiceProvider);
    //     return endpoint.ExecuteAsync(request, cancellationToken);
    // }
}