using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Segres.Contracts;
using Segres.Handlers;

namespace Segres;

/// <summary>
/// The builder to setup the conventions of the Segres library.
/// </summary>
public sealed class SegresConventionBuilder
{
    private static readonly Type GenericRequestBehaviorType = typeof(IRequestBehavior<,>); 
    private readonly HashSet<Assembly> _assemblies;
    private readonly HashSet<Type> _behaviorTypes;
    private readonly IServiceCollection _services;
    private ServiceLifetime _serviceLifetime;
    private Type _publisherType;
    private bool _publishInParallel;


    /// <summary>
    /// Creates a new instance of <see cref="SegresConventionBuilder"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    public SegresConventionBuilder(IServiceCollection services)
    {
        _assemblies = new HashSet<Assembly>();
        _behaviorTypes = new HashSet<Type>();
        _services = services;
        _serviceLifetime = ServiceLifetime.Scoped;
        _publisherType = typeof(DefaultPublisherContext);
        _publishInParallel = false;
    }

    /// <summary>
    /// Register all handlers with a specified lifetime. 
    /// </summary>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseLifetime(ServiceLifetime handlerLifetime)
    {
        this._serviceLifetime = handlerLifetime;
        return this;
    }

    /// <summary>
    /// Register a custom <see cref="IPublisherContext"/> to change the behavior publishing <see cref="INotification"/>'s.
    /// </summary>
    /// <typeparam name="TPublisher"><see cref="IPublisherContext"/></typeparam>
    /// <remarks>
    /// This is useful for implementing the Outbox-Pattern.
    /// If a custom <see cref="IPublisherContext"/> is used, ensure that the <see cref="IConsumer"/> is executed at some point to call the matching <see cref="INotificationHandler{TNotification}"/>.
    /// </remarks>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UsePublisherContext<TPublisher>()
        where TPublisher : IPublisherContext
    {
        _publisherType = typeof(TPublisher);
        return this;
    }

    /// <summary>
    /// Configures the behavior of publishing <see cref="INotification"/>'s.
    /// </summary>
    /// <remarks>
    /// When true publishes notifications in parallel, otherwise sequential.
    /// </remarks>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseParallelNotification()
    {
        _publishInParallel = true;
        return this;
    }

    /// <summary>
    /// Register a <see cref="IRequestBehavior{TRequest,TResult}"/>.
    /// </summary>
    /// <param name="behaviorType">The type implementing the <see cref="IRequestBehavior{TRequest,TResult}"/> interface.</param>
    /// <exception cref="ArgumentException">Thrown when the type doesn't implement the <see cref="IRequestBehavior{TRequest,TResult}"/>.</exception>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseBehavior(Type behaviorType)
    {
        var isBehavior = behaviorType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == GenericRequestBehaviorType);
        if (!isBehavior || behaviorType.IsAbstract || behaviorType.IsInterface)
            throw new ArgumentException($"{behaviorType} can't be used as a behavior. The specified type can not be an abstract class or an interface and must implement {GenericRequestBehaviorType}.", nameof(behaviorType));
        
        _behaviorTypes.Add(behaviorType);
        return this;
    }

    /// <summary>
    /// Gets the assembly and all referenced assemblies of the specified type to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="type">The type to get the assembly from.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseReferencedAssemblies(Type type) 
        => UseReferencedAssemblies(type.Assembly);

    /// <summary>
    /// Use the specified the assembly and all referenced assemblies to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseReferencedAssemblies(Assembly assembly)
    {
        var assemblies = assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Append(assembly)
            .Distinct()
            .ToArray();

        return UseAssemblies(assemblies);
    }
    
    /// <summary>
    /// Gets the assemblies of the specified types to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="types">The types to get the assemblies from.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseAssemblies(params Type[] types)
        => UseAssemblies(types.Select(x => x.Assembly).ToArray());

    /// <summary>
    /// Use the specified the assemblies to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            UseAssembly(assembly);

        return UseBehaviorsFromAssemblies();
    }

    /// <summary>
    /// Gets the assembly of the specified type to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="type">The type to get the assembly from.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseAssembly(Type type)
        => UseAssembly(type.Assembly);

    /// <summary>
    /// Use the specified the assembly to register all <see cref="IRequestBehavior{TRequest,TResult}"/> and <see cref="IRequestHandler{TRequest}"/> found.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns><see cref="SegresConventionBuilder"/></returns>
    public SegresConventionBuilder UseAssembly(Assembly assembly)
    {
        if (!_assemblies.Contains(assembly))
            _assemblies.Add(assembly);

        return this;
    }

    internal SegresConvention Build(Action<SegresConventionBuilder>? options = null)
    {
        options?.Invoke(this);
        return new SegresConvention
        {
            Assemblies = _assemblies,
            BehaviorTypes = _behaviorTypes,
            ServiceLifetime = _serviceLifetime,
            Services = _services,
            PublisherType = _publisherType,
            PublishInParallel = _publishInParallel
        };
    }
    
    private SegresConventionBuilder UseBehaviorsFromAssemblies()
    {
        var behaviors = _assemblies.SelectMany(x => x.DefinedTypes)
            .Where(x => x is {IsAbstract: false, IsInterface: false})
            .Where(x => x.GetInterfaces().Any(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == GenericRequestBehaviorType));
        
        foreach (var definedType in behaviors)
            UseBehavior(definedType);
                
        return this;
    }
}