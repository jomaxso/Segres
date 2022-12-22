using System.Collections.Concurrent;
using System.Reflection;

namespace PlayGround.DependencyInjection;

public class SegresContainer : IServiceProvider
{
    private readonly IDictionary<Type, ServiceDescriptor> _serviceDescriptors;

    public SegresContainer(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        _serviceDescriptors = serviceDescriptors
            .ToDictionary(key => key.ServiceType, value => value);
    }

    public T? GetService<T>()
        where T : class
    {
        return GetService(typeof(T)) as T;
    }

    public T GetRequiredService<T>()
        where T : class
    {
        return (T) GetService(typeof(T))!;
    }

    public object? GetService(Type serviceType)
    {
        if (_serviceDescriptors.TryGetValue(serviceType, out var descriptor) is false)
            throw new Exception($"Service of type {serviceType.Name} isn't registered.");

        if (descriptor.Implementation is not null)
            return descriptor.Implementation;

        var actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (actualType.IsAbstract || actualType.IsInterface)
            throw new Exception("Cannot instantiate abstract classes or interfaces");
        
        var implementation = Activator.CreateInstance(actualType, GetInjectionServices(descriptor.ConstructorParameters));

        if (descriptor.Lifetime == HandlerLifetime.Singleton)
            descriptor.Implementation = implementation;

        return implementation;
    }

    private object?[] GetInjectionServices(ParameterInfo[] parameterInfos)
    {
        var length = parameterInfos.Length;
        var parameters = new object?[length];

        for (var i = 0; i < length; i++)
            parameters[i] = GetService(parameterInfos[i].ParameterType);

        return parameters;
    }
}