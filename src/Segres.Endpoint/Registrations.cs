using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Segres.Contracts;

namespace Segres.Endpoint;
//
// public static class EndpointMappings
// {
//     private static readonly Type ThisType = typeof(EndpointMappings);
//     private static readonly MethodInfo DeleteMethod = ThisType.GetMethod(nameof(MapDeleteEndpoint), BindingFlags.Public | BindingFlags.Static)!;
//     private static readonly MethodInfo GetMethod = ThisType.GetMethod(nameof(MapGetEndpoint), BindingFlags.Public | BindingFlags.Static)!;
//     private static readonly MethodInfo PostMethod = ThisType.GetMethod(nameof(MapPostEndpoint), BindingFlags.Public | BindingFlags.Static)!;
//     private static readonly MethodInfo PutMethod = ThisType.GetMethod(nameof(MapPutEndpoint), BindingFlags.Public | BindingFlags.Static)!;
//
//     public static WebApplication MapEndpoints(this WebApplication application)
//     {
//         var assembly = Assembly.GetCallingAssembly();
//         application.MapEndpoints(assembly);
//
//         return application;
//     }
//
//     private static void MapEndpoints(this IEndpointRouteBuilder application, Assembly assembly)
//     {
//         var requestTypes = assembly.GetRequestTypes();
//
//         foreach (var (requestType, endpointAttribute) in requestTypes)
//         {
//             application.CreateEndpoint(requestType, endpointAttribute.Route)
//                 .WithTags(endpointAttribute.Group ?? endpointAttribute.Route.Split('/')[0]);
//         }
//     }
//
//     private static IEnumerable<ValueTuple<Type, EndpointAttribute>> GetRequestTypes(this Assembly assembly)
//     {
//         return assembly.DefinedTypes
//             .Where(classType => classType.IsDefined(typeof(EndpointAttribute)))
//             .SelectMany(x => x.GetInterfaces().Select(interfaceType => (interfaceType, x.GetCustomAttribute<EndpointAttribute>()!)))
//             .Where(x => x.interfaceType.IsGenericType)
//             .Where(x =>
//                 x.interfaceType.GetGenericTypeDefinition() == typeof(IHttpGetEndpoint<>) ||
//                 x.interfaceType.GetGenericTypeDefinition() == typeof(IHttpPostEndpoint<>) ||
//                 x.interfaceType.GetGenericTypeDefinition() == typeof(IHttpDeleteEndpoint<>) ||
//                 x.interfaceType.GetGenericTypeDefinition() == typeof(IHttpPutEndpoint<>))
//             .Select(x => (x.interfaceType.GetGenericArguments()[0], x.Item2));
//     }
//
//     private static RouteHandlerBuilder CreateEndpoint(this IEndpointRouteBuilder builder, Type requestType, string pattern)
//     {
//         foreach (var @interface in requestType.GetInterfaces())
//         {
//             if (@interface == typeof(IHttpGetRequest))
//                 return builder.MapEndpointToRoute(requestType, GetMethod, pattern);
//
//             if (@interface == typeof(IHttpPostRequest))
//                 return builder.MapEndpointToRoute(requestType, PostMethod, pattern);
//
//             if (@interface == typeof(IHttpPutRequest))
//                 return builder.MapEndpointToRoute(requestType, PutMethod, pattern);
//
//             if (@interface == typeof(IHttpDeleteRequest))
//                 return builder.MapEndpointToRoute(requestType, DeleteMethod, pattern);
//         }
//
//         throw new NotImplementedException();
//     }
//
//     private static RouteHandlerBuilder MapEndpointToRoute(this IEndpointRouteBuilder builder, Type requestType, MethodInfo mapMethod, string pattern)
//     {
//         var method = mapMethod.MakeGenericMethod(requestType);
//         var methodDelegate = (Func<IEndpointRouteBuilder, string, RouteHandlerBuilder>) method.CreateDelegate(typeof(Func<IEndpointRouteBuilder, string, RouteHandlerBuilder>));
//         return methodDelegate.Invoke(builder, pattern);
//     }
//
//
//     private static RouteHandlerBuilder MapGetEndpoint<TRequest>(this IEndpointRouteBuilder builder, string pattern)
//         where TRequest : IHttpGetRequest
//     {
//         return builder.MapGet(pattern, async ([FromServices] IServiceProvider serviceProvider, [FromParameters] TRequest request, CancellationToken cancellationToken)
//             => await EndpointDispatcher.Dispatch(serviceProvider.GetRequiredService, request, cancellationToken));
//     }
//
//
//     private static RouteHandlerBuilder MapPostEndpoint<TRequest>(this IEndpointRouteBuilder builder, string pattern)
//         where TRequest : IHttpPostRequest
//     {
//         return builder.MapPost(pattern, async ([FromServices] IServiceProvider serviceProvider, [FromParameters] TRequest request, CancellationToken cancellationToken)
//             => await EndpointDispatcher.Dispatch(serviceProvider.GetRequiredService, request, cancellationToken));
//     }
//
//     private static RouteHandlerBuilder MapPutEndpoint<TRequest>(this IEndpointRouteBuilder builder, string pattern)
//         where TRequest : IHttpPutRequest
//     {
//         return builder.MapPut(pattern, async ([FromServices] IServiceProvider serviceProvider, [FromParameters] TRequest request, CancellationToken cancellationToken)
//             => await EndpointDispatcher.Dispatch(serviceProvider.GetRequiredService, request, cancellationToken));
//     }
//
//     private static RouteHandlerBuilder MapDeleteEndpoint<TRequest>(this IEndpointRouteBuilder builder, string pattern)
//         where TRequest : IHttpDeleteRequest
//     {
//         return builder.MapDelete(pattern, async ([FromServices] IServiceProvider serviceProvider, [FromParameters] TRequest request, CancellationToken cancellationToken)
//             => await EndpointDispatcher.Dispatch(serviceProvider.GetRequiredService, request, cancellationToken));
//     }
//
//     [AttributeUsage(AttributeTargets.Parameter)]
//     private class FromParametersAttribute : FromBodyAttribute
//     {
//     }
// }
//
// internal static class EndpointDispatcher
// {
//     private static readonly IHandlerCache<HandlerInfo> RequestEndpointCache = new HandlerCache<HandlerInfo>();
//
//     public static Task Dispatch<T>(ServiceResolver serviceResolver, T request, CancellationToken cancellationToken)
//         where T : notnull
//     {
//         var handlerInfo = GetHandlerInfo<T>();
//         var handler = serviceResolver.GetHandler(handlerInfo);
//         return handler.InvokeHttpDeleteDelegate(handlerInfo, request, cancellationToken);
//     }
//
//     private static HandlerInfo GetHandlerInfo<T>()
//         => RequestEndpointCache.FindOrAddHandler(typeof(T), Register);
//
//     private static HandlerInfo Register(Type requestType)
//     {
//         // POST
//         if (requestType.HasInterface(typeof(IHttpPostRequest)))
//         {
//             var endpointType = typeof(IHttpPostEndpoint<>).MakeGenericType(requestType);
//             var endpointDelegate = requestType.CreateHttpPostDelegate();
//             return new HandlerInfo(endpointType, endpointDelegate);
//         }
//
//         // GET
//         if (requestType.HasInterface(typeof(IHttpGetRequest)))
//         {
//             var endpointType = typeof(IHttpGetEndpoint<>).MakeGenericType(requestType);
//             var endpointDelegate = requestType.CreateHttpGetDelegate();
//             return new HandlerInfo(endpointType, endpointDelegate);
//         }
//
//         // PUT
//         if (requestType.HasInterface(typeof(IHttpPutRequest)))
//         {
//             var endpointType = typeof(IHttpPutEndpoint<>).MakeGenericType(requestType);
//             var endpointDelegate = requestType.CreateHttpPutDelegate();
//             return new HandlerInfo(endpointType, endpointDelegate);
//         }
//
//         // DELETE
//         if (requestType.HasInterface(typeof(IHttpDeleteRequest)))
//         {
//             var endpointType = typeof(IHttpDeleteEndpoint<>).MakeGenericType(requestType);
//             var endpointDelegate = requestType.CreateHttpDeleteDelegate();
//             return new HandlerInfo(endpointType, endpointDelegate);
//         }
//
//         throw new AmbiguousImplementationException();
//     }
//
//     private static object GetHandler(this ServiceResolver serviceResolver, HandlerInfo handlerInfo) =>
//         serviceResolver(handlerInfo.Type) ?? throw new Exception("No endpoint registered to handle request.");
//
//     private static Task<IResult> InvokeHttpDeleteDelegate<T>(this object handler, HandlerInfo handlerInfo, T request, CancellationToken cancellationToken)
//         => request switch
//         {
//             IHttpGetRequest r => handlerInfo.Method.InvokeHttpGetDelegate(handler, r, cancellationToken),
//             IHttpPostRequest r => handlerInfo.Method.InvokeHttpPostDelegate(handler, r, cancellationToken),
//             IHttpPutRequest r => handlerInfo.Method.InvokeHttpPutDelegate(handler, r, cancellationToken),
//             IHttpDeleteRequest r => handlerInfo.Method.InvokeHttpDeleteDelegate(handler, r, cancellationToken),
//             _ => throw new ArgumentOutOfRangeException()
//         };
//
//     private static bool HasInterface(this Type type, Type interfaceType)
//     {
//         return type.GetInterfaces().Any(x => x == interfaceType);
//     }
// }
//
// internal static class EndpointDelegates
// {
//     private delegate Task<IResult> HttpGetDelegate(object handler, IHttpGetRequest request, CancellationToken cancellationToken);
//
//     private delegate Task<IResult> HttpPostDelegate(object handler, IHttpPostRequest request, CancellationToken cancellationToken);
//
//     private delegate Task<IResult> HttpPutDelegate(object handler, IHttpPutRequest request, CancellationToken cancellationToken);
//
//     private delegate Task<IResult> HttpDeleteDelegate(object handler, IHttpDeleteRequest request, CancellationToken cancellationToken);
//
//     private static readonly Type HttpGetDelegateType = typeof(DynamicHttpGetHandler<>);
//     private static readonly Type HttpPostDelegateType = typeof(DynamicHttpPostHandler<>);
//     private static readonly Type HttpPutDelegateType = typeof(DynamicHttpPutHandler<>);
//     private static readonly Type HttpDeleteDelegateType = typeof(DynamicHttpDeleteHandler<>);
//
//
//     public static Task<IResult> InvokeHttpGetDelegate(this Delegate endpointDelegate, object handler, IHttpGetRequest request, CancellationToken cancellationToken)
//         => endpointDelegate.ResolveMethod<HttpGetDelegate>().Invoke(handler, request, cancellationToken);
//
//     public static Task<IResult> InvokeHttpPostDelegate(this Delegate endpointDelegate, object handler, IHttpPostRequest request, CancellationToken cancellationToken)
//         => endpointDelegate.ResolveMethod<HttpPostDelegate>().Invoke(handler, request, cancellationToken);
//
//     public static Task<IResult> InvokeHttpPutDelegate(this Delegate endpointDelegate, object handler, IHttpPutRequest request, CancellationToken cancellationToken)
//         => endpointDelegate.ResolveMethod<HttpPutDelegate>().Invoke(handler, request, cancellationToken);
//
//     public static Task<IResult> InvokeHttpDeleteDelegate(this Delegate endpointDelegate, object handler, IHttpDeleteRequest request, CancellationToken cancellationToken)
//         => endpointDelegate.ResolveMethod<HttpDeleteDelegate>().Invoke(handler, request, cancellationToken);
//
//     public static Delegate CreateHttpGetDelegate(this Type requestType) => HttpGetDelegateType.CreateDelegate<HttpGetDelegate>(requestType);
//     public static Delegate CreateHttpPostDelegate(this Type requestType) => HttpPostDelegateType.CreateDelegate<HttpPostDelegate>(requestType);
//     public static Delegate CreateHttpPutDelegate(this Type requestType) => HttpPutDelegateType.CreateDelegate<HttpPutDelegate>(requestType);
//     public static Delegate CreateHttpDeleteDelegate(this Type requestType) => HttpDeleteDelegateType.CreateDelegate<HttpDeleteDelegate>(requestType);
//
//     private static TDelegate CreateDelegate<TDelegate>(this Type dynamicHandlerType, Type requestType)
//         where TDelegate : Delegate => (TDelegate) dynamicHandlerType
//         .MakeGenericType(requestType)
//         .GetMethod("HandleDynamicAsync")!
//         .CreateDelegate(typeof(TDelegate));
//
//     private static TDelegate ResolveMethod<TDelegate>(this Delegate endpointDelegate)
//         where TDelegate : Delegate => (TDelegate) endpointDelegate;
//
//     private static class DynamicHttpGetHandler<TRequest>
//         where TRequest : IHttpGetRequest
//     {
//         public static Task<IResult>? HandleDynamicAsync(object obj, IHttpGetRequest request, CancellationToken cancellationToken)
//         {
//             var handler = obj as IHttpGetEndpoint<TRequest>;
//             return handler?.ExecuteAsync((TRequest) request, cancellationToken);
//         }
//     }
//
//     private static class DynamicHttpPostHandler<TRequest>
//         where TRequest : IHttpPostRequest
//     {
//         public static Task<IResult>? HandleDynamicAsync(object obj, IHttpPostRequest request, CancellationToken cancellationToken)
//         {
//             var handler = obj as IHttpPostEndpoint<TRequest>;
//             return handler?.ExecuteAsync((TRequest) request, cancellationToken);
//         }
//     }
//
//     private static class DynamicHttpPutHandler<TRequest>
//         where TRequest : IHttpPutRequest
//     {
//         public static Task<IResult>? HandleDynamicAsync(object obj, IHttpPutRequest request, CancellationToken cancellationToken)
//         {
//             var handler = obj as IHttpPutEndpoint<TRequest>;
//             return handler?.ExecuteAsync((TRequest) request, cancellationToken);
//         }
//     }
//
//     private static class DynamicHttpDeleteHandler<TRequest>
//         where TRequest : IHttpDeleteRequest
//     {
//         public static Task<IResult>? HandleDynamicAsync(object obj, IHttpDeleteRequest request, CancellationToken cancellationToken)
//         {
//             var handler = obj as IHttpDeleteEndpoint<TRequest>;
//             return handler?.ExecuteAsync((TRequest) request, cancellationToken);
//         }
//     }
// }

public static class Registrations
{
    public static WebApplication MapEndpoints(this WebApplication application, params Type[] markers)
    {
        return application;
    }

    public static RouteHandlerBuilder MapPost<TRequest>(this IEndpointRouteBuilder builder, string pattern)
        where TRequest : IPostRequest
    {
        return builder.MapPost(pattern, (ISender sender, TRequest request, CancellationToken cancellationToken) 
            => sender.SendAsync(request, cancellationToken));
    }

    public static RouteHandlerBuilder MapGet<TRequest>(this IEndpointRouteBuilder builder, string pattern)
        where TRequest : IGetRequest
    {
        return builder.MapGet(pattern, (ISender sender, TRequest request, CancellationToken cancellationToken) 
            => sender.SendAsync(request, cancellationToken));
    }

    public static RouteHandlerBuilder MapPut<TRequest>(this IEndpointRouteBuilder builder, string pattern)
        where TRequest : IPutRequest
    {
        return builder.MapPut(pattern, (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) 
            => sender.SendAsync(request, cancellationToken));
    }
    
    public static RouteHandlerBuilder MapDelete<TRequest>(this IEndpointRouteBuilder builder, string pattern)
        where TRequest : IDeleteRequest
    {
        return builder.MapDelete(pattern, (ISender sender, [AsParameters] TRequest request, CancellationToken cancellationToken) 
            => sender.SendAsync(request, cancellationToken));
    }
    
    [AttributeUsage(AttributeTargets.Parameter)]
    private class AsParametersAttribute : FromBodyAttribute
    {
    }
}