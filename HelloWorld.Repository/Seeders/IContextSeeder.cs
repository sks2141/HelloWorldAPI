using System.Threading.Tasks;

namespace HelloWorld.DataAccess.Seeders
{
    public interface IContextSeeder
    {
        Task PopulateSeedData();
    }
}