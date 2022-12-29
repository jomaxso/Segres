using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Segres.Abstractions;

namespace Segres.AspNetCore;

public static class EndpointExtensions
{
    private static readonly MethodInfo RegisterHandlerMethod = typeof(EndpointExtensions).GetMethod(nameof(RegisterHandlerAsEndpoint), BindingFlags.Static | BindingFlags.NonPublic)!;
    
    public static IApplicationBuilder UseSegres(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder
            .MapEndpoints();
    }

    private static IApplicationBuilder MapEndpoints(this IApplicationBuilder applicationBuilder)
    {
        var options = applicationBuilder.ApplicationServices.GetRequiredService<SegresConfiguration>();

        var types = options.Assemblies.SelectMany(y => y.DefinedTypes
                .Where(x => x is {IsAbstract: false, IsInterface: false})
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestEndpoint<,>))))
            .ToDictionary(
                x => x, 
                y => y.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>)));

        foreach (var keyValues in types)
        {
            foreach (var interfaceType in keyValues.Value)
            {
                var arguments = interfaceType.GetGenericArguments();
                var definitionType = typeof(RequestEndpointDefinition<,>).MakeGenericType(arguments);
                var definition = (IEndpointDefinition) Activator.CreateInstance(definitionType, applicationBuilder)!;

                var args = arguments.Append(keyValues.Key).ToArray();
                
                RegisterHandlerMethod!.MakeGenericMethod(args)
                    .Invoke(null, new object?[]{definition});
            }
        }

        return applicationBuilder;
    }

    private static void RegisterHandlerAsEndpoint<TRequest, TResponse, TEndpoint>(IEndpointDefinition endpointDefinition)
        where TEndpoint : IAsyncRequestEndpoint<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        => TEndpoint.Configure(endpointDefinition);
}