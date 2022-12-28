using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Segres.DependencyInjection;

public interface IServiceInstaller
{
    public static abstract void Install(IServiceCollection services, IConfiguration? configuration);
}