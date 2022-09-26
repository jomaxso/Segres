using System.Reflection;
using DispatchR.Contracts;

namespace DispatchR;

internal static class Helper
{
    public static HandlerCache<HandlerInfo> GetRequestHandlerDetails(this ReadOnlySpan<Assembly> assemblies)
    {
        var dic = new HandlerCache<HandlerInfo>();

        foreach (var requestHandlerDetail in assemblies.GetQueryRequestHandlerDetails_2())
            dic.Add(requestHandlerDetail);

        foreach (var requestHandlerDetail in assemblies.GetCommandRequestHandlerDetails_2())
            dic.Add(requestHandlerDetail);

        foreach (var requestHandlerDetail in assemblies.GetCommandRequestHandlerDetails_1())
            dic.Add(requestHandlerDetail);

        // ValidateRegistrations(assemblies, dic);

        return dic;
    }

    public static HandlerCache<HandlerInfo[]> GetSubscriberDetails(this ReadOnlySpan<Assembly> assemblies)
    {
        var messageHandlerDetails = new Dictionary<Type, List<Type>>();

        var subscriberDetails = assemblies.GetHandlerDetails(typeof(IMessageHandler<>));

        foreach (var handlerDetail in subscriberDetails)
        {
            if (messageHandlerDetails.ContainsKey(handlerDetail.Key))
            {
                messageHandlerDetails[handlerDetail.Key].Add(handlerDetail.Value);
                continue;
            }

            messageHandlerDetails.Add(handlerDetail.Key, new List<Type>() {handlerDetail.Value});
        }

        return messageHandlerDetails.ToHandlerCache((x, values) =>
        {
            List<HandlerInfo> results = new();

            foreach (var value in values)
            {
                var method = typeof(Delegates).GetMethod(nameof(Delegates.CreateMessageDelegate))!;
                var del = (Delegate) method.Invoke(null, new object?[] {x})!;

                results.Add(new HandlerInfo(value, del));
            }

            return results.ToArray();
        });
    }

    private static HandlerCache<HandlerInfo> GetQueryRequestHandlerDetails_2(this ReadOnlySpan<Assembly> assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(IQueryHandler<,>))
            .ToHandlerCache((requestType, handlerType) =>
            {
                var responseType = requestType.GetInterface(typeof(IQuery<>).Name)!.GetGenericArguments()[0];

                var method = typeof(Delegates).GetMethod(nameof(Delegates.CreateQueryDelegate));
                var del = (Delegate) method!.MakeGenericMethod(responseType).Invoke(null, new object?[] {requestType})!;

                return new HandlerInfo(handlerType, del);
            });
    }

    private static HandlerCache<HandlerInfo> GetCommandRequestHandlerDetails_1(this ReadOnlySpan<Assembly> assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(ICommandHandler<>))
            .ToHandlerCache((requestType, handlerType) =>
            {
                var method = typeof(Delegates).GetMethod(nameof(Delegates.CreateCommandWithoutResponseDelegate))!;
                var del = (Delegate) method.Invoke(null, new object?[] {requestType})!;

                return new HandlerInfo(handlerType, del);
            });
    }

    private static HandlerCache<HandlerInfo> GetCommandRequestHandlerDetails_2(this ReadOnlySpan<Assembly> assemblies)
    {
        return assemblies.GetHandlerDetails(typeof(ICommandHandler<,>))
            .ToHandlerCache((requestType, handlerType) =>
            {
                var responseType = requestType.GetInterface(typeof(ICommand<>).Name)!.GetGenericArguments()[0];

                var method = typeof(Delegates).GetMethod(nameof(Delegates.CreateCommandDelegate));
                var del = (Delegate) method!.MakeGenericMethod(responseType).Invoke(null, new object?[] {requestType})!;

                return new HandlerInfo(handlerType, del);
            });
    }


    internal static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(this ReadOnlySpan<Assembly> assemblies, Type type)
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