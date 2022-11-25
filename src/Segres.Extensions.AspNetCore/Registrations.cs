using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Segres;
using Segres.Tmp.Http;

namespace WeatherForecastDemo.Api.Endpoints;

public static class Registrations
{
    private static readonly Type GenericInterfaceType = typeof(IEndpoint<,>);

    private static readonly List<(Type RequestType, Type ResponseType)> GenericRequestTypes = new();

    private static readonly MethodInfo MapEndpointMethod = typeof(Registrations)
        .GetMethod(nameof(MapEndpoint), BindingFlags.Static | BindingFlags.NonPublic)!;


    public static IEndpointRouteBuilder MapSegres<TRequest>(this IEndpointRouteBuilder app) where TRequest : IHttpRequest<IEnumerable<int>>
    {
        var requestType = typeof(TRequest);
        var requestEndpoint = requestType.GetCustomAttributes().OfType<EndpointAttribute>().First();

        
        var routeGroupBuilder = app.MapGroup(requestEndpoint.GroupName).WithTags(requestEndpoint.GroupName);
        var endpoint = routeGroupBuilder.MapGet(requestEndpoint.RoutePattern!,
            ([FromServices] IEndpoint<TRequest, IEnumerable<int>> endpoint, [AsParameters] TRequest request, CancellationToken cancellationToken)
                => endpoint.ExecuteAsync(request, cancellationToken));
        
        return routeGroupBuilder;
    }
    
    public static RouteGroupBuilder MapGroupedEndpoints(this IEndpointRouteBuilder app, string group, Action<RouteGroupBuilder> builder)
    {
        var routeGroupBuilder = app.MapGroup(group).WithTags(group);
        
        builder(routeGroupBuilder);
        return routeGroupBuilder;
    }
    
    public static RouteHandlerBuilder MapGetEndpoint<TRequest, TResponse>(this RouteGroupBuilder routeGroupBuilder, [StringSyntax("Route")] string route = "/")
        where TRequest : IHttpRequest<TResponse>
    {
        var endpoint = routeGroupBuilder.MapGet(route,
            ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [AsParameters] TRequest request, CancellationToken cancellationToken)
                => endpoint.ExecuteAsync(request, cancellationToken));

        return endpoint;
    }

    public static RouteHandlerBuilder MapPostEndpoint<TRequest, TResponse>(this RouteGroupBuilder routeGroupBuilder, [StringSyntax("Route")] string route = "/")
        where TRequest : IHttpRequest<TResponse>
    {
        var endpoint = routeGroupBuilder.MapPost(route,
            ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [FromBody] TRequest request, CancellationToken cancellationToken)
                => endpoint.ExecuteAsync(request, cancellationToken));

        return endpoint;
    }

    public static RouteHandlerBuilder MapPutEndpoint<TRequest, TResponse>(this RouteGroupBuilder routeGroupBuilder, [StringSyntax("Route")] string route = "/")
        where TRequest : IHttpRequest<TResponse>
    {
        var endpoint = routeGroupBuilder.MapPut(route,
            ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [AsParameters] TRequest request, CancellationToken cancellationToken)
                => endpoint.ExecuteAsync(request, cancellationToken));

        return endpoint;
    }

    public static RouteHandlerBuilder MapDeleteEndpoint<TRequest, TResponse>(this RouteGroupBuilder routeGroupBuilder, [StringSyntax("Route")] string route = "/")
        where TRequest : IHttpRequest<TResponse>
    {
        var endpoint = routeGroupBuilder.MapDelete(route,
            ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [AsParameters] TRequest request, CancellationToken cancellationToken)
                => endpoint.ExecuteAsync(request, cancellationToken));

        return endpoint;
    }


    public static void WithEndpoints(this RegistrationOption options)
    {
        var endpointsToRegister = options
            .Assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(x => !x.IsAbstract && !x.IsInterface)
            .Where(x => x.ImplementedInterfaces.Any(IsGenericEndpoint))
            .Distinct()
            .ToList();

        foreach (var endpointType in endpointsToRegister)
            options.Services.AddEndpoint(endpointType);
    }

    public static void UseSegres(this IEndpointRouteBuilder applicationBuilder, Action<EndpointOptions>? options = null)
    {
        var opt = new EndpointOptions();
        options?.Invoke(opt);
        applicationBuilder.MapEndpoints(opt);
    }

    private static void MapEndpoints(this IEndpointRouteBuilder applicationBuilder, EndpointOptions options)
    {
        foreach (var genericRequestType in GenericRequestTypes)
        {
            MapEndpointMethod.MakeGenericMethod(genericRequestType.RequestType, genericRequestType.ResponseType)
                .Invoke(null, new object?[] {applicationBuilder, options});
        }
    }

    private static void MapEndpoint<TRequest, TResponse>(this IEndpointRouteBuilder applicationBuilder, EndpointOptions options)
        where TRequest : IHttpRequest<TResponse>
    {
        var requestType = typeof(TRequest);
        var requestEndpoint = requestType.GetCustomAttributes().OfType<EndpointAttribute>().First();

        var routeGroupBuilder = applicationBuilder
            .MapGroup(requestEndpoint?.GroupName ?? string.Empty)
            .WithTags(requestEndpoint?.GroupName ?? string.Empty);

        var route = requestEndpoint?.RoutePattern ?? string.Empty;

        switch (requestEndpoint.HttpMethod)
        {
            case Http.POST:
                var x = routeGroupBuilder.MapPost(route,
                    ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [FromBody] TRequest request, CancellationToken cancellationToken)
                        => endpoint.ExecuteAsync(request, cancellationToken));
                break;

            case Http.PUT:
            case Http.GET:
            case Http.DELETE:
            default:
                routeGroupBuilder.MapMethods(route, new[] {requestEndpoint.HttpMethod.ToString()},
                    ([FromServices] IEndpoint<TRequest, TResponse> endpoint, [AsParameters] TRequest request, CancellationToken cancellationToken)
                        => endpoint.ExecuteAsync(request, cancellationToken));
                break;
        }
    }

    private static void AddEndpoint(this IServiceCollection services, Type endpointType)
    {
        var endpointInterfaceTypes = endpointType.GetInterfaces().Where(IsGenericEndpoint);

        foreach (var endpointInterfaceType in endpointInterfaceTypes)
        {
            var interfaceArguments = endpointInterfaceType.GetGenericArguments();
            var requestType = (interfaceArguments[0], interfaceArguments[1]);

            if (GenericRequestTypes.Count(x => x == requestType) > 1)
                continue;

            GenericRequestTypes.Add(requestType);

            var interfaceType = GenericInterfaceType.MakeGenericType(requestType.Item1, requestType.Item2);
            services.AddScoped(interfaceType, endpointType);
        }
    }

    private static bool IsGenericEndpoint(Type x)
        => x.IsGenericType && x.GetGenericTypeDefinition() == GenericInterfaceType;
}