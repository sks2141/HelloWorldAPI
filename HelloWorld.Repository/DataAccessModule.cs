using Microsoft.Extensions.DependencyInjection;
using HelloWorld.DataAccess.Contexts;
using HelloWorld.DataAccess.Seeders;

namespace HelloWorld.DataAccess
{
    public class DataAccessModule : IDataAccessModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>();

            services.AddSingleton<IContextSeeder, ContextSeeder>();

            services.AddSingleton<IRepository, Repository>();
        }

        public void Configure(IContextSeeder seeder)
        {
            seeder.PopulateSeedData().Wait();
        }
    }
}