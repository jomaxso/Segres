using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Segres.Handlers;

namespace Segres.AspNetCore;

/// <summary>
/// Extensions to register endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Register all endpoints found by <see cref="IHttpRequest{TResponse}"/> found.
    /// </summary>
    /// <param name="routeBuilder">The <see cref="IEndpointRouteBuilder"/>.</param>
    /// <remarks>
    /// It uses the assemblies registered by the <see cref="Segres.ServiceRegistration.AddSegres()"/>
    /// </remarks>
    public static void UseSegres(this IEndpointRouteBuilder routeBuilder)
    {
        var registerEndpointMethod = typeof(EndpointExtensions).GetMethod(nameof(RegisterEndpoint), BindingFlags.Static | BindingFlags.NonPublic)!;

        var serviceProvider = routeBuilder.ServiceProvider;
        var segresConfiguration = serviceProvider.GetRequiredService<SegresConvention>();

        segresConfiguration.Assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(x => x is {IsAbstract: false, IsInterface: false, IsGenericType: false})
            .Where(x => x.GetInterfaces().Any(xx => xx.HasGenericInterface(typeof(IHttpRequest<>))))
            .SelectMany(requestType => requestType.GetInterfaces()
                .Where(interfaceType => interfaceType.HasGenericInterface(typeof(IHttpRequest<>)))
                .Select(interfaceType => new KeyValuePair<Type,Type>(requestType, interfaceType.GenericTypeArguments[0])))
            .ToList()
            .ForEach(x =>
            {
                var resultType = typeof(HttpResult<>).MakeGenericType(x.Value);
                var handlerType = typeof(IRequestHandler<,>).MakeGenericType(x.Key, resultType);
                var endpointHandlerObject = serviceProvider.CreateScope().ServiceProvider.GetRequiredService(handlerType);
                registerEndpointMethod.MakeGenericMethod(x.Key, x.Value).Invoke(null, new[] {routeBuilder, endpointHandlerObject});
            });
    }

    private static bool HasGenericInterface(this Type type, Type genericInterfaceType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericInterfaceType;

    private static void RegisterEndpoint<TRequest, TResponse>(IEndpointRouteBuilder routeBuilder, IBaseEndpoint<TRequest, TResponse> endpointHandler)
        where TRequest : IHttpRequest<TResponse>
    {
        var routeHandlerBuilder = TRequest.RequestType switch
        {
            RequestType.Get => routeBuilder.MapGet(TRequest.RequestRoute, CreateEndpointAsParameter<TRequest, TResponse>),
            RequestType.Post => routeBuilder.MapPost(TRequest.RequestRoute, CreateEndpointFromBody<TRequest, TResponse>),
            RequestType.Put => routeBuilder.MapPut(TRequest.RequestRoute, CreateEndpointAsParameter<TRequest, TResponse>),
            RequestType.Delete => routeBuilder.MapDelete(TRequest.RequestRoute, CreateEndpointAsParameter<TRequest, TResponse>),
            _ => throw new UnreachableException()
        };

        endpointHandler.Configure(routeHandlerBuilder);
    }

    private static async ValueTask<object> CreateEndpointAsParameter<TRequest, TResponse>(ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken)
        where TRequest : IHttpRequest<TResponse>
    {
        var result = await sender.SendAsync(request, cancellationToken);
        return result.Result;
    }

    private static async ValueTask<object> CreateEndpointFromBody<TRequest, TResponse>(ISender sender, [FromBody] TRequest request, CancellationToken cancellationToken)
        where TRequest : IHttpRequest<TResponse>
    {
        var result = await sender.SendAsync(request, cancellationToken);
        return result.Result;
    }
}