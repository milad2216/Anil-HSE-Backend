using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Anil.Core.Infrastructure;

namespace Website.Infrastructure
{
    /// <summary>
    /// Represents the registering services on application startup
    /// </summary>
    public partial class AnilStartup : IAnilStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            //common factories
            //services.AddScoped<IAclSupportedModelFactory, AclSupportedModelFactory>();

            

            //factories
            //services.AddScoped<Factories.IAddressModelFactory, Factories.AddressModelFactory>();

            //helpers classes
            //services.AddScoped<ITinyMceHelper, TinyMceHelper>();
        }

        // <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2002;
    }
}
