using System.Threading.Tasks;
using HelloWorld.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.DataAccess.Contexts
{
    public class Context : DbContext
    {
        private IConfigurationRoot config;

        public Context(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase();
            //optionsBuilder.UseSqlServer(config["ConnectionStrings:HelloWorldContextConnection"]);
        }

        public DbSet<ResponseEntity> Responses { get; set; }

        public DbSet<StatsEntity> Stats { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}