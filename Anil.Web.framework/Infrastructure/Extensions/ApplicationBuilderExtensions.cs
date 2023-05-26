using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Anil.Core;
using Anil.Core.Configuration;
using Anil.Core.Infrastructure;
using Anil.Data;
using Anil.Data.Migrations;
//using Anil.Web.Framework.Mvc.Routing;
using WebMarkupMin.AspNetCore7;
using Anil.Web.Framework.Mvc.Routing;
using Anil.Core.Http;
using QuestPDF.Drawing;
using Anil.Services.Authentication;
using Anil.Services.Common;
using Anil.Services.Seo;
using Anil.Core.Domain.Common;
using Anil.Services.Logging;
using Microsoft.AspNetCore.Connections;
using CKSource.CKFinder.Connector.Host.Owin;
using CKSource.CKFinder.Connector.Core.Builders;
using CKSource.CKFinder.Connector.Config;
using CKSource.FileSystem.Local;
using CKSource.CKFinder.Connector.Core.Acl;
using CKSource.CKFinder.Connector.KeyValue.FileSystem;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.BuilderProperties;
using Microsoft.AspNetCore.DataProtection;

namespace Anil.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }

        public static void StartEngine(this IApplicationBuilder application)
        {
            var engine = EngineContext.Current;

            //log application start
            engine.Resolve<ILogger>().Information("Application started");

            //update nopCommerce core and db
            var migrationManager = engine.Resolve<IMigrationManager>();
            var assembly = Assembly.GetAssembly(typeof(ApplicationBuilderExtensions));
            migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
            assembly = Assembly.GetAssembly(typeof(IMigrationManager));
            migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
        }

        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilExceptionHandler(this IApplicationBuilder application)
        {
            var appSettings = EngineContext.Current.Resolve<AppSettings>();
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
            var useDetailedExceptionPage = appSettings.Get<CommonConfig>().DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
            if (useDetailedExceptionPage)
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                application.UseExceptionHandler("/Error/Error");
            }

            //log errors
            application.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return;

                    try
                    {
                        //log error
                        await EngineContext.Current.Resolve<ILogger>().ErrorAsync(exception.Message, exception);
                    }
                    finally
                    {
                        //rethrow the exception to show the error page
                        ExceptionDispatchInfo.Throw(exception);
                    }
                });
            });
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 404 status code that do not have a body
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UsePageNotFound(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                //handle 404 Not Found
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    if (!webHelper.IsStaticResource())
                    {
                        //get original path and query
                        var originalPath = context.HttpContext.Request.Path;
                        var originalQueryString = context.HttpContext.Request.QueryString;


                        var appSettings = Singleton<AppSettings>.Instance;
                        var commonSettings = appSettings.Get<CommonSettings>();

                        if (commonSettings.Log404Errors)
                        {
                            var logger = EngineContext.Current.Resolve<ILogger>();
                            var workContext = EngineContext.Current.Resolve<IWorkContext>();

                            await logger.ErrorAsync($"Error 404. The requested page ({originalPath}) was not found");
                        }

                        try
                        {
                            if (context.HttpContext.Request.Path.ToString().ToLower().Contains("api"))
                            {
                                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                            }
                            else
                            {
                                //get new path
                                var pageNotFoundPath = "/page-not-found";
                                //re-execute request with new path
                                context.HttpContext.Response.Redirect(context.HttpContext.Request.PathBase + pageNotFoundPath);
                            }
                        }
                        finally
                        {
                            //return original path to request
                            context.HttpContext.Request.QueryString = originalQueryString;
                            context.HttpContext.Request.Path = originalPath;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 400 status code (bad request)
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBadRequestResult(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                //handle 404 (Bad request)
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    var logger = EngineContext.Current.Resolve<ILogger>();
                    var workContext = EngineContext.Current.Resolve<IWorkContext>();
                    await logger.ErrorAsync("Error 400. Bad request", null);
                }
            });
        }

        /// <summary>
        /// Configure middleware for dynamically compressing HTTP responses
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilResponseCompression(this IApplicationBuilder application)
        {
            //whether to use compression (gzip by default)
            application.UseResponseCompression();
        }
        /// <summary>
        /// Configure static file serving
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilStaticFiles(this IApplicationBuilder application)
        {
            var fileProvider = EngineContext.Current.Resolve<IAnilFileProvider>();
            var appSettings = EngineContext.Current.Resolve<AppSettings>();

            void staticFileResponse(StaticFileResponseContext context)
            {
                if (!string.IsNullOrEmpty(appSettings.Get<CommonConfig>().StaticFilesCacheControl))
                    context.Context.Response.Headers.Append(HeaderNames.CacheControl, appSettings.Get<CommonConfig>().StaticFilesCacheControl);
            }

            //add handling if sitemaps 
            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(AnilSeoDefaults.SitemapXmlDirectory)),
                RequestPath = new Microsoft.AspNetCore.Http.PathString($"/{AnilSeoDefaults.SitemapXmlDirectory}"),
                //OnPrepareResponse = context =>
                //{
                //    if (!DataSettingsManager.IsDatabaseInstalled() ||
                //        !EngineContext.Current.Resolve<SitemapXmlSettings>().SitemapXmlEnabled)
                //    {
                //        context.Context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //        context.Context.Response.ContentLength = 0;
                //        context.Context.Response.Body = Stream.Null;
                //    }
                //}
            });

            //common static files
            application.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });


            //add support for backups
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings = { [".bak"] = MimeTypes.ApplicationOctetStream }
            };

            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(AnilCommonDefaults.DbBackupsPath)),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/db_backups"),
                ContentTypeProvider = provider,
                //OnPrepareResponse = context =>
                //{
                //    if (!DataSettingsManager.IsDatabaseInstalled() ||
                //        !EngineContext.Current.Resolve<IPermissionService>().AuthorizeAsync(StandardPermissionProvider.ManageMaintenance).Result)
                //    {
                //        context.Context.Response.StatusCode = StatusCodes.Status404NotFound;
                //        context.Context.Response.ContentLength = 0;
                //        context.Context.Response.Body = Stream.Null;
                //    }
                //}
            });

            //add support for webmanifest files
            provider.Mappings[".webmanifest"] = MimeTypes.ApplicationManifestJson;

            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath("icons")),
                RequestPath = "/icons",
                ContentTypeProvider = provider
            });

            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(".well-known")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/.well-known"),
                ServeUnknownFileTypes = true,
            });
        }

        /// <summary>
        /// Adds the authentication middleware, which enables authentication capabilities.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilAuthentication(this IApplicationBuilder application)
        {
            application.UseMiddleware<AuthenticationMiddleware>();
            application.UseAuthentication();
        }

        /// <summary>
        /// Configure PDF
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilPdf(this IApplicationBuilder application)
        {
            var fileProvider = EngineContext.Current.Resolve<IAnilFileProvider>();
            var fontPaths = fileProvider.EnumerateFiles(fileProvider.MapPath("~/App_Data/Pdf/"), "*.ttf") ?? Enumerable.Empty<string>();

            //write placeholder characters instead of unavailable glyphs for both debug/release configurations
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            foreach (var fp in fontPaths)
            {
                FontManager.RegisterFont(File.OpenRead(fp));
            }
        }

        /// <summary>
        /// Configure the request localization feature
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilRequestLocalization(this IApplicationBuilder application)
        {
            application.UseRequestLocalization(async options =>
            {
                //prepare supported cultures
                options.DefaultRequestCulture = new RequestCulture("fa");
                options.ApplyCurrentCultureToResponseHeaders = true;

                //configure culture providers
                //options.AddInitialRequestCultureProvider(new AnilSeoUrlCultureProvider());
                var cookieRequestCultureProvider = options.RequestCultureProviders.OfType<CookieRequestCultureProvider>().FirstOrDefault();
                if (cookieRequestCultureProvider is not null)
                    cookieRequestCultureProvider.CookieName = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.CultureCookie}";
            });
        }

        /// <summary>
        /// Configure Endpoints routing
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilEndpoints(this IApplicationBuilder application)
        {
            //Execute the endpoint selected by the routing middleware
            application.UseEndpoints(endpoints =>
            {
                //register all routes
                EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(endpoints);
            });
        }

        /// <summary>
        /// Configure applying forwarded headers to their matching fields on the current request.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilProxy(this IApplicationBuilder application)
        {
            var appSettings = EngineContext.Current.Resolve<AppSettings>();

            if (appSettings.Get<HostingConfig>().UseProxy)
            {
                var options = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All,
                    // IIS already serves as a reverse proxy and will add X-Forwarded headers to all requests,
                    // so we need to increase this limit, otherwise, passed forwarding headers will be ignored.
                    ForwardLimit = 2
                };

                if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().ForwardedForHeaderName))
                    options.ForwardedForHeaderName = appSettings.Get<HostingConfig>().ForwardedForHeaderName;

                if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().ForwardedProtoHeaderName))
                    options.ForwardedProtoHeaderName = appSettings.Get<HostingConfig>().ForwardedProtoHeaderName;

                if (!string.IsNullOrEmpty(appSettings.Get<HostingConfig>().KnownProxies))
                {
                    foreach (var strIp in appSettings.Get<HostingConfig>().KnownProxies.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                    {
                        if (IPAddress.TryParse(strIp, out var ip))
                            options.KnownProxies.Add(ip);
                    }

                    if (options.KnownProxies.Count > 1)
                        options.ForwardLimit = null; //disable the limit, because KnownProxies is configured
                }

                //configure forwarding
                application.UseForwardedHeaders(options);
            }
        }

        /// <summary>
        /// Configure WebMarkupMin
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseAnilWebMarkupMin(this IApplicationBuilder application)
        {
            application.UseWebMarkupMin();
        }

        public static Action<IApplicationBuilder> SetupConnector(this IApplicationBuilder app)
        {
            
            Action<IApplicationBuilder> action = (app) =>
            {
                app.UseOwin(setup => setup(next =>
                {
                    var builder = new AppBuilder();

                    SetupCKFinderConnector(builder);

                    return builder.Build<Func<IDictionary<string, object>, Task>>();
                }));
            };
            return action;
        }


        public static void SetupCKFinderConnector(this IAppBuilder app)
        {
            /*
             * Create a connector instance using ConnectorBuilder. The call to the LoadConfig() method
             * will configure the connector using CKFinder configuration options defined in Web.config.
             */
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();

            /*
             * Create an instance of authenticator implemented in the previous step.
             */
            //var customAuthenticator = new CustomCKFinderAuthenticator();


            connectorBuilder
                /*
                 * Provide the global configuration.
                 *
                 * If you installed CKSource.CKFinder.Connector.Config, you should load the static configuration
                 * from XML:
                 * connectorBuilder.LoadConfig();
                 */
                //.LoadConfig()
                //.SetAuthenticator(customAuthenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        /*
                         * If you installed CKSource.CKFinder.Connector.Config, you might want to load the static
                         * configuration from XML as a base configuration to modify:
                         */
                        //config.LoadConfig();

                        /*
                         * Configure settings per request.
                         *
                         * The minimal configuration has to include at least one backend, one resource type
                         * and one ACL rule.
                         *
                         * For example:
                         */
                        config.AddBackend("default", new LocalStorage(@"C:\files"));
                        config.AddResourceType("images", builder => builder.SetBackend("default", "images"));
                        config.AddAclRule(new AclRule(
                            new StringMatcher("*"),
                            new StringMatcher("*"),
                            new StringMatcher("*"),
                            new Dictionary<Permission, PermissionType> { { Permission.All, PermissionType.Allow } }));


                        /*
                         * If you installed CKSource.CKFinder.Connector.KeyValue.FileSystem, you may enable caching:
                         */
                        var defaultBackend = config.GetBackend("default");
                        var keyValueStoreProvider = new FileSystemKeyValueStoreProvider(defaultBackend);
                        config.SetKeyValueStoreProvider(keyValueStoreProvider);
                    }
                );

            /*
             * Build the connector middleware.
             */
            var connector = connectorBuilder
                .Build(connectorFactory);

            /*
             * Add the CKFinder connector middleware to the web application pipeline.
             */

            app.UseConnector(connector);
        }
    }
}
