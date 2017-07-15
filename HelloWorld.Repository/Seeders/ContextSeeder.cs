using HelloWorld.Model;
using HelloWorld.DataAccess.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.DataAccess.Seeders
{
    public class ContextSeeder : IContextSeeder
    {
        private Context context;
        
        public ContextSeeder(Context context)
        {
            this.context = context;
        }

        // Can easily extend this to pull from database instead
        public async Task PopulateSeedData()
        {
            if (!this.context.Responses.Any())
            {
                this.context.Responses.AddRange(new List<ResponseEntity>
                {
                    new ResponseEntity()
                    {
                        Id = 1,
                        Message = "Hello World"
                    },
                    new ResponseEntity()
                    {
                        Id = 2,
                        Message = "Hello Earth"
                    }
                });
            }

            await this.context.SaveChangesAsync();
        }
    }
}