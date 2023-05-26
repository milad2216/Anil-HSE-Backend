using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Configuration;
using Anil.Core.Infrastructure;
using Anil.Data;
using Anil.Web.Framework.Menu;
using Anil.Web.framework;
using Anil.Web.Framework.Mvc.Routing;
using Anil.Services.Logging;
using Anil.Core.Events;
using Anil.Services.Events;
using System.Reflection;
using Anil.Services.Menus;
using Anil.Services.Blogs;
using Anil.Services.Seo;
using Anil.Services.Configuration;
using Anil.Services.Duties;
namespace Anil.Web.Framework.Infrastructure
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
            //file provider
            services.AddScoped<IAnilFileProvider, AnilFileProvider>();

            //web helper
            services.AddScoped<IWebHelper, WebHelper>();

            //user agent helper
            //services.AddScoped<IUserAgentHelper, UserAgentHelper>();

            //static cache manager
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

            //work context
            services.AddScoped<IWorkContext, WebWorkContext>();

            //services
            services.AddScoped<ILogger, DefaultLogger>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IBlogPostViewService, BlogPostViewService>();
            services.AddScoped<IDutyService, DutyService>();
            services.AddScoped<IBlogCommentService, BlogCommentService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IUrlRecordService, UrlRecordService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IActivityLogTypeService, ActivityLogTypeService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddSingleton<IRoutePublisher, RoutePublisher>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #region CodeGen

            #endregion


            //register all settings
            var typeFinder = Singleton<ITypeFinder>.Instance;

            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting, serviceProvider =>
                {

                    return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
                });
            }

            services.AddScoped<SlugRouteTransformer>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);

            //XML sitemap
            services.AddScoped<IXmlSiteMap, XmlSiteMap>();

            services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2000;
    }
}