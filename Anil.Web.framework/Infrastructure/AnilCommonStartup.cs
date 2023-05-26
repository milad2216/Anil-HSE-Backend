using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Anil.Core.Infrastructure;
using Anil.Web.Framework.Infrastructure.Extensions;
using Anil.Web.Framework.Mvc.Routing;
//using Anil.Web.Framework.Mvc.Routing;

namespace Anil.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public partial class AnilCommonStartup : IAnilStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add options feature
            services.AddOptions();

            //add distributed cache
            services.AddDistributedCache();

            //add HTTP sesion state feature
            services.AddHttpSession();

            //add anti-forgery
            services.AddAntiForgery();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //check whether requested page is keep alive page
            //application.UseKeepAlive();

            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseAnilRequestLocalization();

            //configure PDF
            application.UseAnilPdf();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 100; //common services should be loaded after error handlers
    }
}