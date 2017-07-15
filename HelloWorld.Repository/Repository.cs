using HelloWorld.Model;
using HelloWorld.DataAccess.Contexts;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace HelloWorld.DataAccess
{
    public class Repository : IRepository
    {
        private readonly Context context;
        private ILogger<Repository> logger;

        public Repository(Context context, ILogger<Repository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<ResponseEntity> GetResponses()
        {
            this.logger.LogInformation("Getting all responses from Repository.");

            return this.context.Responses;
        }
    }
}