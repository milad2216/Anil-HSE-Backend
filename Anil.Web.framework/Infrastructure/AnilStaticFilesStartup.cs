using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Anil.Core.Infrastructure;
using Anil.Web.Framework.Infrastructure.Extensions;

namespace Anil.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents class for the configuring routing on application startup
    /// </summary>
    public partial class AnilStaticFilesStartup : IAnilStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //compression
            services.AddResponseCompression();

            //middleware for bundling and minification of CSS and JavaScript files.
            //services.AddAnilWebOptimizer();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use response compression before UseAnilStaticFiles to support compress for it
            application.UseAnilResponseCompression();

            //use static files feature
            application.UseAnilStaticFiles();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 99; //Static files should be registered before routing & custom middlewares
    }
}