using System.Reflection;
using System.Runtime.CompilerServices;

namespace Segres;

internal static class DelegatesExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Delegate CreateInternalDelegate(this Type selfType, string methodName, params Type[] methodParameter)
    {
        var method = selfType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        return (Delegate) method!.MakeGenericMethod(methodParameter).Invoke(null, Array.Empty<object>())!;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? GetService(this Func<Type, object?> serviceResolver, Type type) 
        => serviceResolver.Invoke(type);
}