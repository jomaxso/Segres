using System.Reflection;
using MicrolisR.Pipelines;

namespace MicrolisR;

internal static class Helper
{
    public static IDictionary<Type, (Type Type, Delegate Del)> GetRequestHandlerDetails(this Assembly[] assemblies)
    {
        IDictionary<Type, (Type Type, Delegate Del)> dic = new Dictionary<Type, (Type Type, Delegate Del)>();

        foreach (var requestHandlerDetail in assemblies.GetQueryRequestHandlerDetails_2())
            dic.Add(requestHandlerDetail);

        foreach (var requestHandlerDetail in assemblies.GetCommandRequestHandlerDetails_2())
            dic.Add(requestHandlerDetail);

        foreach (var requestHandlerDetail in assemblies.GetCommandRequestHandlerDetails_1())
            dic.Add(requestHandlerDetail);

        return dic;
    }

    private static IDictionary<Type, (Type Type, Delegate Del)> GetQueryRequestHandlerDetails_2(this Assembly[] assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(IQueryRequestHandler<,>))
            .ToDictionary(x => x.Key, x =>
            {
                var responseType = x.Key.GetInterface(typeof(IQueryRequest<>).Name)!.GetGenericArguments()[0];

                var method = typeof(DynamicHandler).GetMethod(nameof(DynamicHandler.CreateQueryDelegate));
                var del = (Delegate) method!.MakeGenericMethod(responseType).Invoke(null, new object?[] {x.Key})!;

                return (x.Value, del);
            });
    }

    private static IDictionary<Type, (Type Type, Delegate Del)> GetCommandRequestHandlerDetails_1(this Assembly[] assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(ICommandRequestHandler<>))
            .ToDictionary(x => x.Key, x =>
            {
                var method = typeof(DynamicHandler).GetMethod(nameof(DynamicHandler.CreateCommandDelegate))!;
                var del = (Delegate) method.Invoke(null, new object?[] {x.Key})!;

                return (x.Value, del);
            });
    }

    private static IDictionary<Type, (Type Type, Delegate Del)> GetCommandRequestHandlerDetails_2(this Assembly[] assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(ICommandRequestHandler<,>))
            .ToDictionary(x => x.Key, x =>
            {
                var responseType = x.Key.GetInterface(typeof(ICommandRequest<>).Name)!.GetGenericArguments()[0];

                var method = typeof(DynamicHandler).GetMethod(nameof(DynamicHandler.CreateCommandDelegate));
                var del = (Delegate) method!.MakeGenericMethod(responseType).Invoke(null, new object?[] {x.Key})!;

                return (x.Value, del);
            });
    }

    public static IDictionary<Type, Type[]> GetSubscriberDetails(this Assembly[] assemblies)
    {
        var messageHandlerDetails = new Dictionary<Type, List<Type>>();

        var subscriberDetails = assemblies.GetHandlerDetails(typeof(INotificationHandler<>));

        foreach (var handlerDetail in subscriberDetails)
        {
            if (messageHandlerDetails.ContainsKey(handlerDetail.Key))
            {
                messageHandlerDetails[handlerDetail.Key].Add(handlerDetail.Value);
                continue;
            }

            messageHandlerDetails.Add(handlerDetail.Key, new List<Type>() {handlerDetail.Value});
        }

        return messageHandlerDetails.ToDictionary(x => x.Key, x => x.Value.ToArray());
    }

    public static IDictionary<Type, Type[]> GetPipelineDetails(this Assembly[] assemblies)
    {
        var messageHandlerDetails = new Dictionary<Type, List<Type>>();

        var pipelines = assemblies.GetHandlerDetails(typeof(IPipelineBehavior<,>));

        foreach (var handlerDetail in pipelines)
        {
            if (messageHandlerDetails.ContainsKey(handlerDetail.Key))
            {
                messageHandlerDetails[handlerDetail.Key].Add(handlerDetail.Value);
                continue;
            }

            messageHandlerDetails.Add(handlerDetail.Key, new List<Type>() {handlerDetail.Value});
        }

        return messageHandlerDetails.ToDictionary(x => x.Key, x => x.Value.ToArray());
    }

    internal static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(this IEnumerable<Assembly> assemblies, Type type)
    {
        var handlerDetails = new List<KeyValuePair<Type, Type>>();

        foreach (var assembly in assemblies)
        {
            var handlers = GetHandlerDetails(assembly, type);
            handlerDetails.AddRange(handlers);
        }

        return handlerDetails;
    }

    private static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(this Assembly assembly, Type type)
    {
        var classesImplementingInterface = GetClassesImplementingInterface(assembly, type);

        var details = new List<KeyValuePair<Type, Type>>();

        foreach (var value in classesImplementingInterface)
        {
            var types = value
                .GetInterfaces()
                .Where(x => x.Name == type.Name)
                .Select(x => x.GetGenericArguments()[0]);

            foreach (var key in types)
            {
                details.Add(new KeyValuePair<Type, Type>(key, value));
            }
        }

        return details;
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(this Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

            return !type.IsInterface && !type.IsAbstract && isImplementRequestType;
        }).ToList();
    }
}