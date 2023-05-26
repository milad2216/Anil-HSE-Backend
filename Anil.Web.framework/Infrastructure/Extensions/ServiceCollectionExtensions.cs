using System;
using System.Linq;
using System.Net;
using Azure.Identity;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Anil.Core;
using Anil.Core.Configuration;
using Anil.Core.Infrastructure;
using Anil.Core.Security;
using Anil.Data;
using StackExchange.Profiling.Storage;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;
using Anil.Core.Http;
using Anil.Web.Framework.Mvc.Routing;
using Anil.Web.Framework.Mvc.ModelBinding;
using Anil.Web.Framework.Mvc.ModelBinding.Binders;
using Anil.Services.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Anil.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationSettings(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //create default file provider
            CommonHelper.DefaultFileProvider = new AnilFileProvider(builder.Environment);

            //register type finder
            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationServices(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //initialize plugins
            var mvcCoreBuilder = services.AddMvcCore();
            
            //create engine and configure service provider
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, builder.Configuration);
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.AntiforgeryCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAnilAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            ////set default authentication schemes
            //var authenticationBuilder = services.AddAuthentication(options =>
            //{
            //    options.DefaultChallengeScheme = AnilAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultScheme = AnilAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = AnilAuthenticationDefaults.ExternalAuthenticationScheme;
            //});

            ////add main cookie authentication
            //authenticationBuilder.AddCookie(AnilAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.Cookie.Name = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.AuthenticationCookie}";
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    options.LoginPath = AnilAuthenticationDefaults.LoginPath;
            //    options.AccessDeniedPath = AnilAuthenticationDefaults.AccessDeniedPath;
            //});

            ////add external authentication
            //authenticationBuilder.AddCookie(AnilAuthenticationDefaults.ExternalAuthenticationScheme, options =>
            //{
            //    options.Cookie.Name = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.ExternalAuthenticationCookie}";
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    options.LoginPath = AnilAuthenticationDefaults.LoginPath;
            //    options.AccessDeniedPath = AnilAuthenticationDefaults.AccessDeniedPath;
            //});
            services.AddAuthentication().AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]??"")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddAnilMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddControllersWithViews();

            mvcBuilder.AddRazorRuntimeCompilation();

            var appSettings = Singleton<AppSettings>.Instance;
            if (appSettings.Get<CommonConfig>().UseSessionStateTempDataProvider)
            {
                //use session-based temp data provider
                mvcBuilder.AddSessionStateTempDataProvider();
            }
            else
            {
                //use cookie-based temp data provider
                mvcBuilder.AddCookieTempDataProvider(options =>
                {
                    options.Cookie.Name = $"{AnilCookieDefaults.Prefix}{AnilCookieDefaults.TempDataCookie}";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });
            }

            services.AddRazorPages();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //set some options
            mvcBuilder.AddMvcOptions(options =>
            {
                //we'll use this until https://github.com/dotnet/aspnetcore/issues/6566 is solved 
                options.ModelBinderProviders.Insert(0, new InvariantNumberModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new CustomPropertiesModelBinderProvider());
                //add custom display metadata provider 
                options.ModelMetadataDetailsProviders.Add(new AnilMetadataProvider());

                //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
                //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
                //options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => AnilValidationDefaults.NotNullValidationLocaleName);
            });

            //add fluent validation
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            //register all available validators from Anil assemblies
            var assemblies = mvcBuilder.PartManager.ApplicationParts
                .OfType<AssemblyPart>()
                .Where(part => part.Name.StartsWith("Anil", StringComparison.InvariantCultureIgnoreCase))
                .Select(part => part.Assembly);
            services.AddValidatorsFromAssemblies(assemblies);

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAnilRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            services.AddScoped<IActionResultExecutor<RedirectResult>, AnilRedirectResultExecutor>();
        }

        /// <summary>
        /// Add and configure WebMarkupMin service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAnilWebMarkupMin(this IServiceCollection services)
        {
            services
                .AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.DisableMinification = false;
                    options.DisableCompression = true;
                    options.DisablePoweredByHttpHeaders = true;
                })
                .AddHtmlMinification(options =>
                {
                    options.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes;

                    options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                    options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                })
                .AddXmlMinification(options =>
                {
                    var settings = options.MinificationSettings;
                    settings.RenderEmptyTagsWithSpace = true;
                    settings.CollapseTagsWithoutContent = true;
                });
        }
    }
}