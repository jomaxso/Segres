using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.AspNet;

public static class EndpointExtensions
{
    public static TResult Match<T, TResult>(this Result<T> result, Func<T, TResult> success,  Func<Error, TResult> failure)
    {
        return result.IsSuccess ? success(result.Value) : failure(result.Error);
    }
    
    public static IApplicationBuilder UseSegres(this IApplicationBuilder applicationBuilder)
    {
        var options = applicationBuilder.ApplicationServices.GetRequiredService<SegresConfiguration>();
        var serviceProvider = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider;

        var types = options.Assemblies.SelectMany(y => y.DefinedTypes
            .Where(x => x is {IsAbstract: false, IsInterface: false})
            .Where(x => x.GetInterfaces().Any(i => i == typeof(IEndpointConfiguration)))
            .SelectMany(x => x.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))))
            .ToArray();

        foreach (var type in types)
        {
            var arguments = type.GetGenericArguments();
            var firstArgument = arguments[0].GetGenericArguments()[0];
            var secondArgument = arguments[1];

            var definitionType = typeof(RequestEndpointDefinition<,>).MakeGenericType(firstArgument, secondArgument);

            if (Activator.CreateInstance(definitionType, applicationBuilder) is not EndpointDefinition endpointDefinition)
                continue;

            if (serviceProvider.GetRequiredService(type) is not IEndpointConfiguration endpointConfiguration)
                continue;

            endpointConfiguration.Configure(endpointDefinition);
        }

        return applicationBuilder;
    }
}