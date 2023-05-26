using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Anil.Web.Framework.Mvc.Routing;

namespace Website.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //get language pattern
            //it's not needed to use language pattern in AJAX requests and for actions returning the result directly (e.g. file to download),
            //use it only for URLs of pages that the user can go to
            //var lang = GetLanguageRoutePattern();

            //areas
            endpointRouteBuilder.MapControllerRoute(name: "areaRoute",
                pattern: $"{{area:exists}}/{{controller=Home}}/{{action=Get}}/{{id?}}");

            //home page
            endpointRouteBuilder.MapControllerRoute(name: "Homepage",
                pattern: $"/",
                defaults: new { controller = "Home", action = "Index" });

            //services
            endpointRouteBuilder.MapControllerRoute(name: "service-list",
                pattern: $"services",
                defaults: new { controller = "Service", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "service-details",
                pattern: $"service/{{DutyId:min(0)}}",
                defaults: new { controller = "Service", action = "Details" });
            
            //blogs
            endpointRouteBuilder.MapControllerRoute(name: "blog-list",
                pattern: $"blogs",
                defaults: new { controller = "Blog", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "blog-details",
                pattern: $"blog/{{PostId:min(0)}}",
                defaults: new { controller = "Blog", action = "Details" });

            //change language
            endpointRouteBuilder.MapControllerRoute(name: "ChangeLanguage",
                pattern: $"changelanguage/{{langid:min(0)}}",
                defaults: new { controller = "Common", action = "SetLanguage" });

            //robots.txt (file result)
            endpointRouteBuilder.MapControllerRoute(name: "robots.txt",
                pattern: $"robots.txt",
                defaults: new { controller = "Common", action = "RobotsTextFile" });

            //sitemap
            endpointRouteBuilder.MapControllerRoute(name: "Sitemap",
                pattern: $"sitemap",
                defaults: new { controller = "Common", action = "Sitemap" });

            //sitemap.xml (file result)
            endpointRouteBuilder.MapControllerRoute(name: "sitemap.xml",
                pattern: $"sitemap.xml",
                defaults: new { controller = "Common", action = "SitemapXml" });

            endpointRouteBuilder.MapControllerRoute(name: "sitemap-indexed.xml",
                pattern: $"sitemap-{{Id:min(0)}}.xml",
                defaults: new { controller = "Common", action = "SitemapXml" });

            //error page
            endpointRouteBuilder.MapControllerRoute(name: "Error",
                pattern: $"error",
                defaults: new { controller = "Common", action = "Error" });

            //page not found
            endpointRouteBuilder.MapControllerRoute(name: "PageNotFound",
                pattern: $"page-not-found",
                defaults: new { controller = "Common", action = "PageNotFound" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;

        #endregion
    }
}