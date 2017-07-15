using HelloWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HelloWorld.API.Controllers
{
    [Route("api/[controller]")]
    public class HelloWorldController : Controller
    {
        private IService service;
        private ILogger<HelloWorldController> logger;

        public HelloWorldController(IService service, ILogger<HelloWorldController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET api/helloworld
        [HttpGet]
        // GET api/helloworld/1
        [HttpGet("{idParam}")]
        public string Get(string idParam = null)
        {
            string response = string.Format("Did not find the message, linked to id:{0}", idParam);

            int id = string.IsNullOrEmpty(idParam)? 0 : -1;

            if (string.IsNullOrEmpty(idParam) || (int.TryParse(idParam, out id) && id > -1))
            {
                this.logger.LogInformation("Get() called with input param: id:{0}", id);

                var cachedResponses = this.service.GetResponses().ToList();

                if (cachedResponses.Skip(id).Any())
                {
                    response = cachedResponses[id].Message;
                }

                this.logger.LogInformation("Get()'s output with Responses[0]:{0},\n Output response:{1}",
                                            cachedResponses.Skip(id).Any() ? cachedResponses[id].ToString() : "", response);
                
                // @Future: 
                // User Authentication
                // Log user's details via pushing the ResponseStatsViewModel to repository layer to get statistics of the user activity
            }
            
            return response;
        }

        // POST api/helloworld
        [HttpPost]
        public void Post([FromBody]string value)
        {
            //NOP
        }

        // PUT api/helloworld/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //NOP
        }

        // DELETE api/helloworld/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //NOP
        }
    }
}