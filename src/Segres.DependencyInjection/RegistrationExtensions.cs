using System.Reflection;

namespace Segres.Extensions;

internal static class RegistrationExtensions
{
    public static IEnumerable<KeyValuePair<Type, Type>> GetGenericHandlers(this IEnumerable<Assembly> assemblies, Type type)
    {
        return assemblies.GetClassesImplementingInterface(type)
            .Distinct()
            .SelectMany(implementationType =>
            {
                return implementationType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                    .Select(i => i.GetGenericArguments())
                    .Select(i => new KeyValuePair<Type, Type>(type.MakeGenericType(i), implementationType));
            });
    }

    public static IEnumerable<Assembly> AppendReferencedAssemblies(this Assembly assembly)
    {
        return assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Append(assembly)
            .Distinct();
    }

    private static IEnumerable<Type> GetClassesImplementingInterface(this IEnumerable<Assembly> assemblies, Type typeToMatch)
        => assemblies.SelectMany(assembly => GetClassesImplementingInterface(assembly, typeToMatch));

    private static IEnumerable<Type> GetClassesImplementingInterface(this Assembly assembly, Type typeToMatch)
    {
        return assembly.DefinedTypes.Where(type =>
        {
            var isImplementRequestType = type
                .GetInterfaces()
                .Where(x => x.IsGenericType)
                .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

            return type.IsInstance() && isImplementRequestType;
        });
    }

    private static bool IsInstance(this Type type) 
        => type is {IsInterface: false, IsAbstract: false};
}