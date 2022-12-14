using System.Collections.Concurrent;

namespace Segres;

internal static class DictionaryExtensions
{
    public static T GetOrAdd<T>(this ConcurrentDictionary<Type, object> dictionary, Type type) where T : IHandlerDefinition<T> 
        => (T)dictionary.GetOrAdd(type, static x => T.Create(x));
}