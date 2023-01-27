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

    /// <inheritdoc cref="IServiceCollection"/>
    public required IServiceCollection Services { get; init; }

    /// <inheritdoc cref="ServiceLifetime"/>
    public required ServiceLifetime ServiceLifetime { get; init; }
}