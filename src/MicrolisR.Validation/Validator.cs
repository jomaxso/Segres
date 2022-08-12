using System.Reflection;

namespace MicrolisR.Validation;

public class Validator : IValidator
{
    private readonly IDictionary<Type, IValidation[]> _validatorDetails;

    public Validator(IServiceProvider serviceProvider)
        : this(serviceProvider.GetService, Assembly.GetCallingAssembly())
    {
    }

    public Validator(IServiceProvider serviceProvider, params Type[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    public Validator(IServiceProvider serviceProvider, params Assembly[] markers)
        : this(serviceProvider.GetService, markers)
    {
    }

    public Validator(Func<Type, object?> serviceResolver)
        : this(serviceResolver, Assembly.GetCallingAssembly())
    {
    }

    public Validator(Func<Type, object?> serviceResolver, params Type[] markers)
        : this(serviceResolver, markers.Select(x => x.Assembly).ToArray())
    {
    }

    public Validator(Func<Type, object?> serviceResolver, params Assembly[] markers)
    {
        var validationDetails = GetHandlerDetails(markers, typeof(IValidation<>));

        Dictionary<Type, List<IValidation>> validationHandlerDetails = new();

        foreach (var handlerDetail in validationDetails)
        {
            if (serviceResolver(handlerDetail.Value) is not IValidation validationHandler)
                continue;
            
            if (validationHandlerDetails.ContainsKey(handlerDetail.Key))
            {
                validationHandlerDetails[handlerDetail.Key].Add(validationHandler);
                continue;
            }

            validationHandlerDetails.Add(handlerDetail.Key, new List<IValidation>() {validationHandler});
        }

        _validatorDetails = validationHandlerDetails.ToDictionary(x => x.Key, x => x.Value.ToArray());
    }


    public void Validate<T>(T value)
        where T : IValidatable
    {
        var objectType = value.GetType();

        if (_validatorDetails.ContainsKey(objectType) is false)
            return;

        var handlers = _validatorDetails[objectType];

        for (var i = 0; i < handlers.Length; i++)
        {
            handlers[i].Validate(value);
        }
    }

    private static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(IEnumerable<Assembly> assemblies, Type type)
    {
        var handlerDetails = new List<KeyValuePair<Type, Type>>();

        foreach (var assembly in assemblies)
        {
            var handlers = GetHandlerDetails(assembly, type);
            handlerDetails.AddRange(handlers);
        }

        return handlerDetails;
    }

    private static IEnumerable<KeyValuePair<Type, Type>> GetHandlerDetails(Assembly assembly, Type type)
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

    private static IEnumerable<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
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