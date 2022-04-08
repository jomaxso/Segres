using MicrolisR.Api;
using MicrolisR.Api.Enumeration;
using MicrolisR.Api.Internals;
using MicrolisR.Api.Internals.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicrolisR.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddEndpointR(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetEntryAssembly();

        if (assembly is null)
            return services;

        var enpoints = assembly.ExportedTypes
            .Where(type =>
            {
                var attribute = type.GetCustomAttribute<ApiEndpointAttribute>();
                return type.IsInterface is false && type.IsAbstract is false && attribute is not null;
            })
            .Select(type =>
            {
                var handler = type;

                var request = handler.GetInterfaces()
                .FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IApiEndpoint<,>))!
                .GetGenericArguments()[0];

                return (handler, request);
            })
            .ToList();

        Dictionary<Type, Type> requestDictionary = new();

        foreach (var (handler, request) in enpoints)
        {
            services.AddSingleton(handler);
            requestDictionary[request] = handler;
        }

        services.AddSingleton(provider => new RequestResolver(provider.GetRequiredService, requestDictionary));

        return services;
    }

    public static void MapEndpointR(this IEndpointRouteBuilder app)
    {
        var endpointObject = app.ServiceProvider.GetRequiredService<RequestResolver>();

        foreach (var item in endpointObject.RequestEndpoints)
        {
            var requestAttribute = item.Value.GetCustomAttribute<ApiEndpointAttribute>();
            var endpointHandler = endpointObject.Resolve(item.Key) as IEndpointHandler;

            if (requestAttribute is null || endpointHandler is null)
                continue;

            IEndpointConventionBuilder endpoint;

            switch (requestAttribute.Kind)
            {
                case RequestKind.HttpGet:
                    endpoint = app.MapGet(requestAttribute.Route, async ctx => await ctx.MapAsync(endpointHandler, item.Key, requestAttribute))
                        ;
                    break;

                case RequestKind.HttpPost:
                    endpoint = app.MapPost(requestAttribute.Route, async ctx => await ctx.MapAsync(endpointHandler, item.Key, requestAttribute));
                    break;

                case RequestKind.HttpPut:
                    endpoint = app.MapPut(requestAttribute.Route, async ctx => await ctx.MapAsync(endpointHandler, item.Key, requestAttribute));
                    break;

                case RequestKind.HttpDelete:
                    endpoint = app.MapDelete(requestAttribute.Route, async ctx => await ctx.MapAsync(endpointHandler, item.Key, requestAttribute));
                    break;

                default:
                    continue;
            }

            // await ctx.MapAsync(endpointHandler, item.Key, requestAttribute)

            if (requestAttribute.AllowAnonymous)
                endpoint.AllowAnonymous();
               
        }
    }

    public static async Task MapAsync(this HttpContext ctx, IEndpointHandler handler, Type requestType, ApiEndpointAttribute endpointAttribute)
    {
        try
        {
            var requestObject = await ctx.Request.BindAsync(requestType, endpointAttribute);
            if (requestObject is null)
            {
                var re = Results.BadRequest();
                await re.ExecuteAsync(ctx);
                return;
            }

            var result = await handler.HandleAsync(requestObject, ctx.RequestAborted);
            await ctx.Response.WriteAsJsonAsync(result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
