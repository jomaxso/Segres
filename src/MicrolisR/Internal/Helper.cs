using System.Reflection;
using MicrolisR.Pipelines;

namespace MicrolisR;

internal static class Helper
{
    public static IDictionary<Type, Type> GetRequestHandlerDetails(this Assembly[] assemblies, Type type)
    {
        return assemblies.GetHandlerDetails(type)
            .ToDictionary(x => x.Key, x => x.Value);
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