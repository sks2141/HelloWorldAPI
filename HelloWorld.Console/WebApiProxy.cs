using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HelloWorld.ConsoleApp
{
    public class WebApiProxy
    {
        private readonly HttpClient client;
        private readonly Uri uri;

        public WebApiProxy(IConfigurationRoot config, string urlKey, string timeoutInMilliSecondsKey)
        {
            // ToDo : params validation

            this.client = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(double.Parse(config[timeoutInMilliSecondsKey]))
            };

            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.uri = new Uri(config[urlKey]);
        }

        public async Task<string> Get()
        {
            string result = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(this.uri);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }

            return result;
        }
    }
}