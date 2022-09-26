using System.Runtime.InteropServices;

namespace MicrolisR;

internal class DefaultProvider : IServiceProvider
{
    private readonly bool _asSingleton;
    private static readonly IDictionary<Type, object?> SingletonServices = new Dictionary<Type, object?>();
    private static readonly IDictionary<Type, Type[]> TransientServices = new Dictionary<Type, Type[]>();

    public DefaultProvider(bool asSingleton)
    {
        _asSingleton = asSingleton;
    }

    public object? GetService(Type serviceType)
    {
        return _asSingleton
            ? GetSingletonService(serviceType)
            : GetTransientService(serviceType);
    }

    private static object? GetSingletonService(Type serviceType)
    {
        if (SingletonServices.ContainsKey(serviceType))
            return SingletonServices[serviceType];

        var parameters = serviceType.GetConstructors().First().GetParameters();
        var constructor = parameters.Select(parameter => GetSingletonService(parameter.ParameterType)!).ToArray();

        var implementation = Activator.CreateInstance(serviceType, constructor);
        SingletonServices.Add(serviceType, implementation);

        return implementation;
    }

    private static object? GetTransientService(Type serviceType)
    {
        if (TransientServices.ContainsKey(serviceType) is false)
        {
            var parameterTypes = serviceType
                .GetConstructors()
                .First()
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            TransientServices.Add(serviceType, parameterTypes);
        }

        var parameterTypesSpan = TransientServices[serviceType].AsSpan();
        var length = parameterTypesSpan.Length;
        
        var parameters = new object?[length];

        for (var i = 0; i < length; i++)
        {
            
            parameters[i] = GetTransientService(parameterTypesSpan[i]);
        }

        return Activator.CreateInstance(serviceType, parameters);
    }
}