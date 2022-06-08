using System.ComponentModel;

namespace System.Linq;

[EditorBrowsable(EditorBrowsableState.Never)]
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class LocalQueryMethodImplementationTypeAttribute : Attribute
{
    /// <summary>
    /// Creates a new mapping to the specified local execution query method implementation type.
    /// </summary>
    /// <param name="targetType">Type with query methods for local execution.</param>
    public LocalQueryMethodImplementationTypeAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    /// <summary>
    /// Gets the type with the implementation of local query methods.
    /// </summary>
    public Type TargetType { get; }
}