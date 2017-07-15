using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld.Utilities
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}