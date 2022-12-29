using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Segres;

public interface IServiceInstaller
{
    public static abstract void Install(IServiceCollection services, IConfiguration? configuration);
}