using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Anil.Core.Infrastructure;
using Anil.Web.Framework.Infrastructure.Extensions;

namespace Anil.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring routing on application startup
    /// </summary>
    public partial class AnilRoutingStartup : IAnilStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add MiniProfiler services
            //services.AddAnilMiniProfiler();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use MiniProfiler must come before UseAnilEndpoints
            application.UseMiniProfiler();

            //Add the RoutingMiddleware
            application.UseRouting();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 400; // Routing should be loaded before authentication
    }
}
