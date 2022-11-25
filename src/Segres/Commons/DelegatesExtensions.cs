using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Segres.Tmp;

namespace Segres;

internal static class DelegatesExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Delegate CreateInternalDelegate(this Type selfType, string methodName, params Type[] methodParameter)
    {
        var method = selfType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        return (Delegate) method!.MakeGenericMethod(methodParameter).Invoke(null, Array.Empty<object>())!;
    }
    
    public static Delegate CreateInternalDelegate(this Type selfType, string methodName, Type[] methodParameter, params object[] methodArguments)
    {
        var method = selfType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        return (Delegate) method!.MakeGenericMethod(methodParameter).Invoke(null, methodArguments)!;
    }

    public static T GetOrAdd<T>(this ConcurrentDictionary<Type, object> dictionary, Type type) where T : IRequestConstructor<T> 
        => (T)dictionary.GetOrAdd(type, static x => T.Create(x));
}