using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

/// <summary>
/// Represents the configuration of the Segres library.
/// </summary>
public sealed record SegresConvention 
{
    /// <summary>
    /// A set of assemblies to scan.
    /// </summary>
    public required IReadOnlySet<Assembly> Assemblies { get; init; }
    
    /// <summary>
    /// All <see cref="IRequestBehavior{TRequest,TResult}"/> found after scanning the specified assemblies.
    /// </summary>
    public required IReadOnlySet<Type> BehaviorTypes { get; init; }

    /// <inheritdoc cref="IServiceCollection"/>
    public required IServiceCollection Services { get; init; }

    /// <inheritdoc cref="ServiceLifetime"/>
    public required ServiceLifetime ServiceLifetime { get; init; }

    /// <summary>
    /// The specified context for publishing <see cref="INotification"/>'s with the <see cref="IPublisher"/> interface. 
    /// </summary>
    /// <remarks>
    /// <see cref="PublisherContext"/> has to be implemented.
    /// </remarks>
    public required Type PublisherType { get; init; }
    
    /// <summary>
    /// Configures the behavior of publishing <see cref="INotification"/>'s.
    /// </summary>
    /// <remarks>
    /// When true publishes notifications in parallel, otherwise sequential.
    /// </remarks>
    public required bool PublishInParallel { get; init; }
}