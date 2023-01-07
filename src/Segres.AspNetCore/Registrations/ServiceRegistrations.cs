using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.AspNetCore;

public static class ServiceRegistrations
{
    public static void AddInstallerRegistrations(this ISegresContext registration)
    {
        var classTypes = registration.Assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => IsAssignableToType(typeof(IServiceInstaller), t));

        var method = typeof(ServiceRegistrations).GetMethod(nameof(InstallFromInstaller), BindingFlags.Static | BindingFlags.NonPublic)!;
        var configuration = registration.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        foreach (var classType in classTypes)
        {
            method.MakeGenericMethod(classType)
                .Invoke(null, new object?[] {registration.Services, configuration});
        }


        static bool IsAssignableToType(Type matchingType, TypeInfo typeInfo) =>
            matchingType.IsAssignableFrom(typeInfo) &&
            typeInfo is {IsInterface: false, IsAbstract: false};
    }

    private static void InstallFromInstaller<T>(IServiceCollection services, IConfiguration configuration)
        where T : IServiceInstaller => T.Install(services, configuration);
}