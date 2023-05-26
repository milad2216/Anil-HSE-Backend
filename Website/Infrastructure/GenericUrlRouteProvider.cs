using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Anil.Data;
using Anil.Web.Framework.Mvc.Routing;

namespace Website.Infrastructure
{
    /// <summary>
    /// Represents provider that provided generic routes
    /// </summary>
    public partial class GenericUrlRouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {

            endpointRouteBuilder.MapControllerRoute(name: "Default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //generic routes (actually routing is processed later in SlugRouteTransformer)
            var genericCatalogPattern = $"{{{AnilRoutingDefaults.RouteValue.CatalogSeName}}}/{{{AnilRoutingDefaults.RouteValue.SeName}}}";
            endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(genericCatalogPattern);

            var genericPattern = $"{{{AnilRoutingDefaults.RouteValue.SeName}}}";
            endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(genericPattern);


            endpointRouteBuilder.MapControllerRoute(name: AnilRoutingDefaults.RouteName.Generic.GenericUrl,
                pattern: $"{{{AnilRoutingDefaults.RouteValue.SeName}}}",
                defaults: new { controller = "Common", action = "GenericUrl" });

            endpointRouteBuilder.MapControllerRoute(name: AnilRoutingDefaults.RouteName.Generic.GenericCatalogUrl,
                pattern: $"{{{AnilRoutingDefaults.RouteValue.CatalogSeName}}}/{{{AnilRoutingDefaults.RouteValue.SeName}}}",
                defaults: new { controller = "Common", action = "GenericUrl" });

            //routes for entities that support catalog path and slug (e.g. '/category-seo-name/product-seo-name')
            endpointRouteBuilder.MapControllerRoute(name: AnilRoutingDefaults.RouteName.Generic.ProductCatalog,
                pattern: genericCatalogPattern,
                defaults: new { controller = "Product", action = "ProductDetails" });

            //routes for entities that support single slug (e.g. '/product-seo-name')
            endpointRouteBuilder.MapControllerRoute(name: AnilRoutingDefaults.RouteName.Generic.Product,
                pattern: genericPattern,
                defaults: new { controller = "Product", action = "ProductDetails" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        /// <remarks>
        /// it should be the last route. we do not set it to -int.MaxValue so it could be overridden (if required)
        /// </remarks>
        public int Priority => -1000000;

        #endregion
    }
}