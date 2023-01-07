using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

public interface ISegresContext
{
    public IEnumerable<Assembly> Assemblies { get; }

    /// <inheritdoc cref="IServiceCollection"/>
    public IServiceCollection Services { get; }
}