using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Segres;

internal static class DictionaryExtensions
{
    public static T GetOrAdd<T>(this ConcurrentDictionary<Type, object> dictionary, Type type) where T : IHandlerDefinition<T> 
        => (T)dictionary.GetOrAdd(type, static x => T.Create(x));
    
    public static bool TryGetValue<T>(this IDictionary<Type, object> dictionary, Type key, [NotNullWhen(true)] out T? value) where T : IHandlerDefinition<T>
    {
        var isFound = dictionary.TryGetValue(key, out var innerValue);

        if (isFound && innerValue is T outerValue)
        {
            value = outerValue;
            return true;
        }

        value = default;
        return false;
    }
}