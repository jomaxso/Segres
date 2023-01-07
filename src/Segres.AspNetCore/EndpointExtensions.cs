using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Segres.Abstractions;

namespace Segres.AspNetCore;

public static class EndpointExtensions
{
    public static RouteHandlerBuilder MapGet<TRequest, TResponse>(this IEndpointRouteBuilder routeBuilder, [StringSyntax("Route")] string route)
        where TRequest : IRequest<TResponse>
    {
        return routeBuilder.MapGet(route, CreateEndpointAsParameter<TRequest, TResponse>);
    }
    
    public static RouteHandlerBuilder MapPost<TRequest, TResponse>(this IEndpointRouteBuilder routeBuilder, [StringSyntax("Route")] string route)
        where TRequest : IRequest<TResponse>
    {
        return routeBuilder.MapPost(route, CreateEndpointFromBody<TRequest, TResponse>);
    }
    
    public static RouteHandlerBuilder MapPut<TRequest, TResponse>(this IEndpointRouteBuilder routeBuilder, [StringSyntax("Route")] string route)
        where TRequest : IRequest<TResponse>
    {
        return routeBuilder.MapPut(route, CreateEndpointAsParameter<TRequest, TResponse>);
    }
    
    public static RouteHandlerBuilder MapDelete<TRequest, TResponse>(this IEndpointRouteBuilder routeBuilder, [StringSyntax("Route")] string route)
        where TRequest : IRequest<TResponse>
    {
        return routeBuilder.MapDelete(route, CreateEndpointAsParameter<TRequest, TResponse>);
    }

    private static ValueTask<IResult> CreateEndpointAsParameter<TRequest, TResponse>(ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
        => sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken);
    
    private static ValueTask<IResult> CreateEndpointFromBody<TRequest, TResponse>(ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
        => sender.SendAsync(new EndpointRequest<TRequest, TResponse>(request), cancellationToken);

    // private static readonly MethodInfo RegisterHandlerMethod = typeof(EndpointExtensions).GetMethod(nameof(RegisterHandlerAsEndpoint), BindingFlags.Static | BindingFlags.NonPublic)!;

    // public static IApplicationBuilder UseSegres(this IApplicationBuilder applicationBuilder)
    // {
    //     return applicationBuilder
    //         .MapEndpoints();
    // }
    //
    // private static IApplicationBuilder MapEndpoints(this IApplicationBuilder applicationBuilder)
    // {
    //     var options = applicationBuilder.ApplicationServices.GetRequiredService<ISegresContext>();
    //
    //     var types = options.Assemblies.SelectMany(y => y.DefinedTypes
    //             .Where(x => x is {IsAbstract: false, IsInterface: false})
    //             .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestEndpoint<,>))))
    //         .ToDictionary(
    //             x => x,
    //             y => y.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>)));
    //
    //     foreach (var keyValues in types)
    //     {
    //         foreach (var interfaceType in keyValues.Value)
    //         {
    //             var arguments = interfaceType.GetGenericArguments()[0].GetGenericArguments();
    //             var definitionType = typeof(RequestEndpointDefinition<,>).MakeGenericType(arguments);
    //             var definition = (IEndpointDefinition) Activator.CreateInstance(definitionType, applicationBuilder)!;
    //
    //             var args = arguments.Append(keyValues.Key).ToArray();
    //
    //             RegisterHandlerMethod!.MakeGenericMethod(args)
    //                 .Invoke(null, new object?[] {definition});
    //         }
    //     }
    //
    //     return applicationBuilder;
    // }
    //
    // private static void RegisterHandlerAsEndpoint<TRequest, TResponse, TEndpoint>(IEndpointDefinition endpointDefinition)
    //     where TEndpoint : IAsyncRequestEndpoint<TRequest, TResponse>
    //     where TRequest : IRequest<TResponse>
    //     => TEndpoint.Configure(endpointDefinition);
}