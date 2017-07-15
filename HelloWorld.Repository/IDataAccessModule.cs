using HelloWorld.DataAccess.Seeders;
using HelloWorld.Utilities;

namespace HelloWorld.DataAccess
{
    public interface IDataAccessModule : IModule
    {
        void Configure(IContextSeeder seeder);
    }
}