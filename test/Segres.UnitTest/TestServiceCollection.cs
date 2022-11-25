using Microsoft.Extensions.DependencyInjection;

namespace Segres.UnitTest;

public static class TestServiceCollection
{
    private static readonly IServiceCollection ServiceCollection = new ServiceCollection();

    static TestServiceCollection()
    {
        ServiceCollection.AddSegres();
    }

    public static IServiceProvider CreateServiceProvider()
    {
        return ServiceCollection
            .BuildServiceProvider();
    }
}