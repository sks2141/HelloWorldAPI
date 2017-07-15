using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HelloWorld.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebApiProxy proxy;

        public HomeController(IConfigurationRoot config)
        {
            //ToDo - inject this @later
            proxy = new WebApiProxy(config, "HelloWorldApiUrl", "RequestTimeOutInMilliSeconds");
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Message = await proxy.Get();

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}