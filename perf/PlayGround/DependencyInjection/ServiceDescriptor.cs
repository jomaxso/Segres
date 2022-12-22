using System.Reflection;

namespace PlayGround.DependencyInjection;

public class ServiceDescriptor
{
    public ServiceDescriptor(object implementation, HandlerLifetime lifetime)
    {
        Implementation = implementation;
        Lifetime = lifetime;
        ServiceType = implementation.GetType();
        ConstructorParameters = ServiceType.GetConstructors()[0].GetParameters();
    }

    public ServiceDescriptor(Type serviceType, HandlerLifetime lifetime)
    {
        Lifetime = lifetime;
        ServiceType = serviceType;
        ConstructorParameters = serviceType.GetConstructors()[0].GetParameters();
    }

    public ServiceDescriptor(Type serviceType, Type implementationType, HandlerLifetime lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
        ConstructorParameters = implementationType.GetConstructors()[0].GetParameters();
    }

    public Type ServiceType { get; }
    public Type? ImplementationType { get; }
    
    public ParameterInfo[] ConstructorParameters { get; }
    public object? Implementation { get; internal set; }
    public HandlerLifetime Lifetime { get; }
}