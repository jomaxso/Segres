namespace PlayGround.DependencyInjection;

public sealed class SegresCollection
{
    private readonly List<ServiceDescriptor> _serviceDescriptors = new();

    public SegresCollection RegisterTransient<TService>()
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), HandlerLifetime.Transient));
        return this;
    }

    public SegresCollection RegisterTransient<TService, TImplementation>()
        where TImplementation : TService
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), HandlerLifetime.Transient));
        return this;
    }
    
    public SegresCollection RegisterSingleton<TService, TImplementation>()
        where TImplementation : TService
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), HandlerLifetime.Singleton));
        return this;
    }


    // public void RegisterTransient<TService, TImplementation>(TImplementation implementation)
    // {
    //     _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), implementation, HandlerLifetime.Transient));
    // }

    public SegresCollection RegisterSingleton<TService>()
    {
        _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), HandlerLifetime.Singleton));
        return this;
    }

    public SegresCollection RegisterSingleton<TService>(TService implementation)
        where TService : notnull
    {
        _serviceDescriptors.Add(new ServiceDescriptor(implementation, HandlerLifetime.Singleton));
        return this;
    }

    public SegresContainer BuildContainer()
    {
        return new SegresContainer(_serviceDescriptors);
    }
}