using HelloWorld.DataAccess;
using HelloWorld.DataAccess.Seeders;
using HelloWorld.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace HelloWorld.API
{
    public class Startup
    {
        private IHostingEnvironment env;
        private IConfigurationRoot config;

        private IDataAccessModule repositoryModule;
        private IServiceModule serviceModule;

        public Startup(IHostingEnvironment env)
        {
            this.env = env;
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.config = builder.Build();

            this.repositoryModule = new DataAccessModule();
            this.serviceModule = new ServiceModule();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton(this.config);
            
            services.AddLogging();

            this.repositoryModule.ConfigureServices(services);
            this.serviceModule.ConfigureServices(services);

            services.AddMvc(config =>
            {
                if (this.env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(config => 
            {
                config.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            IContextSeeder seeder,
            ILoggerFactory loggerFactory)
        {
            this.repositoryModule.Configure(seeder);
            this.serviceModule.Configure();

            loggerFactory.AddLog4Net();

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " +
                    context.HttpContext.Response.StatusCode);
            });

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                app.UseExceptionHandler("/error");
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();
            
            app.UseMvc(config =>
            {
                config.MapRoute(
                  name: "Default",
                  template: "{controller}/{action}/{id?}",
                  defaults: new { controller = "Home", action = "Index" }
                  );
            });
        }
    }
}