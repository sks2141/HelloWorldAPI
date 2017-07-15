using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HelloWorld.ConsoleApp
{
    public class Program
    {
        private static IConfigurationRoot config;

        static Program()
        {
            Configure();
        }

        public static void Main(string[] args)
        {
            RunAsync().Wait();

            Console.ReadLine();
        }

        public static async Task RunAsync()
        {
            WebApiProxy proxy = new WebApiProxy(config, "HelloWorldApiUrl", "RequestTimeOutInMilliSeconds");

            Console.WriteLine(await proxy.Get());
        }

        public static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
        }
    }
}