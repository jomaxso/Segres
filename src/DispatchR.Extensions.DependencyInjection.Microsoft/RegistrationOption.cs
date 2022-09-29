using Microsoft.Extensions.DependencyInjection;

namespace DispatchR.Extensions.DependencyInjection.Microsoft;

public record class RegistrationOption
{
    public RegistrationOption()
    {
        QueryHandlerLifetime = ServiceLifetime.Transient;
        CommandHandlerLifetime = ServiceLifetime.Transient;
        MessageHandlerLifetime = ServiceLifetime.Transient;
    }

    internal RegistrationOption(ServiceLifetime queryHandlerLifetime, ServiceLifetime commandHandlerLifetime, ServiceLifetime messageHandlerLifetime)
    {
        QueryHandlerLifetime = queryHandlerLifetime;
        CommandHandlerLifetime = commandHandlerLifetime;
        MessageHandlerLifetime = messageHandlerLifetime;
        StreamHandlerLifetime = messageHandlerLifetime;
    }

    internal ServiceLifetime QueryHandlerLifetime { get; private set; }
    internal ServiceLifetime CommandHandlerLifetime { get; private set; }
    internal ServiceLifetime MessageHandlerLifetime { get; private set; }
    internal ServiceLifetime StreamHandlerLifetime { get; private set; }

    public void AsTransient()
    {
        QueryHandlerLifetime = ServiceLifetime.Transient;
        CommandHandlerLifetime = ServiceLifetime.Transient;
        MessageHandlerLifetime = ServiceLifetime.Transient;
        StreamHandlerLifetime = ServiceLifetime.Transient;
    }

    public void AsScoped()
    {
        QueryHandlerLifetime = ServiceLifetime.Scoped;
        CommandHandlerLifetime = ServiceLifetime.Scoped;
        MessageHandlerLifetime = ServiceLifetime.Scoped;
        StreamHandlerLifetime = ServiceLifetime.Scoped;
    }

    public void AsSingleton()
    {
        QueryHandlerLifetime = ServiceLifetime.Singleton;
        CommandHandlerLifetime = ServiceLifetime.Singleton;
        MessageHandlerLifetime = ServiceLifetime.Singleton;
        StreamHandlerLifetime = ServiceLifetime.Singleton;
    }
}