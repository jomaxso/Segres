using Microsoft.Extensions.DependencyInjection;

namespace Segres.Extensions.DependencyInjection.Microsoft;

/// <summary>
/// 
/// </summary>
public record class RegistrationOption
{
    /// <summary>
    /// 
    /// </summary>
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
    internal Strategy PublishStrategy { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strategy"></param>
    public void UsePublishStartegy(Strategy strategy) => this.PublishStrategy = strategy;

    /// <summary>
    /// 
    /// </summary>
    public void AsTransient()
    {
        QueryHandlerLifetime = ServiceLifetime.Transient;
        CommandHandlerLifetime = ServiceLifetime.Transient;
        MessageHandlerLifetime = ServiceLifetime.Transient;
        StreamHandlerLifetime = ServiceLifetime.Transient;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AsScoped()
    {
        QueryHandlerLifetime = ServiceLifetime.Scoped;
        CommandHandlerLifetime = ServiceLifetime.Scoped;
        MessageHandlerLifetime = ServiceLifetime.Scoped;
        StreamHandlerLifetime = ServiceLifetime.Scoped;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AsSingleton()
    {
        QueryHandlerLifetime = ServiceLifetime.Singleton;
        CommandHandlerLifetime = ServiceLifetime.Singleton;
        MessageHandlerLifetime = ServiceLifetime.Singleton;
        StreamHandlerLifetime = ServiceLifetime.Singleton;
    }
}