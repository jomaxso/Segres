using System.Reflection;

namespace Demo;

internal class AssemblyMarker
{
    private AssemblyMarker(){}

    public static Type Type { get; } = typeof(AssemblyMarker);
    public static Assembly Assembly { get; } = Type.Assembly;
}