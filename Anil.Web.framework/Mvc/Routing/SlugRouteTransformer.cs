using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Seo;
using Anil.Core.Events;
using Anil.Core.Http;
using Anil.Services.Seo;
using Anil.Core.Domain.Logging;
using Anil.Web.Framework.Events;
using Anil.Core.Domain.Duties;

namespace Anil.Web.Framework.Mvc.Routing
{
    /// <summary>
    /// Represents slug route transformer
    /// </summary>
    public partial class SlugRouteTransformer : DynamicRouteValueTransformer
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public SlugRouteTransformer(IEventPublisher eventPublisher,
            IUrlRecordService urlRecordService)
        {
            _eventPublisher = eventPublisher;
            _urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Transform route values according to the passed URL record
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="values">The route values associated with the current match</param>
        /// <param name="urlRecord">Record found by the URL slug</param>
        /// <param name="catalogPath">URL catalog path</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task SingleSlugRoutingAsync(HttpContext httpContext, RouteValueDictionary values, UrlRecord urlRecord, string catalogPath)
        {
            //if URL record is not active let's find the latest one
            var slug = urlRecord.IsActive
                ? urlRecord.Slug
                : await _urlRecordService.GetActiveSlugAsync(urlRecord.EntityId, urlRecord.EntityName);
            if (string.IsNullOrEmpty(slug))
                return;

            if (!urlRecord.IsActive)
            {
                //permanent redirect to new URL with active single slug
                InternalRedirect(httpContext, values, $"/{slug}", true);
                return;
            }

            //since we are here, all is ok with the slug, so process URL
            switch (urlRecord.EntityName)
            {
                case var name when name.Equals(nameof(Log), StringComparison.InvariantCultureIgnoreCase):
                    RouteToAction(values, "Catalog", "ProductsByTag", slug, (AnilRoutingDefaults.RouteValue.ProductTagId, urlRecord.EntityId));
                    return;

                case var name when name.Equals(nameof(BlogPost), StringComparison.InvariantCultureIgnoreCase):
                    if(catalogPath.ToLower() != "blog")
                    {
                        //permanent redirect to new URL with active single slug
                        InternalRedirect(httpContext, values, $"/blog/{slug}", true);
                        return;
                    }
                    RouteToAction(values, "Blog", "Details", slug, (AnilRoutingDefaults.RouteValue.BlogPostId, urlRecord.EntityId));
                    return;

                case var name when name.Equals(nameof(Duty), StringComparison.InvariantCultureIgnoreCase):
                    if (catalogPath.ToLower() != "service")
                    {
                        //permanent redirect to new URL with active single slug
                        InternalRedirect(httpContext, values, $"/service/{slug}", true);
                        return;
                    }
                    RouteToAction(values, "Service", "Details", slug, (AnilRoutingDefaults.RouteValue.DutyId, urlRecord.EntityId));
                    return;
            }
        }

        /// <summary>
        /// Transform route values to redirect the request
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="values">The route values associated with the current match</param>
        /// <param name="path">Path</param>
        /// <param name="permanent">Whether the redirect should be permanent</param>
        protected virtual void InternalRedirect(HttpContext httpContext, RouteValueDictionary values, string path, bool permanent)
        {
            values[AnilRoutingDefaults.RouteValue.Controller] = "Common";
            values[AnilRoutingDefaults.RouteValue.Action] = "InternalRedirect";
            values[AnilRoutingDefaults.RouteValue.Url] = $"{httpContext.Request.PathBase}{path}{httpContext.Request.QueryString}";
            values[AnilRoutingDefaults.RouteValue.PermanentRedirect] = permanent;
            httpContext.Items[AnilHttpDefaults.GenericRouteInternalRedirect] = true;
        }

        /// <summary>
        /// Transform route values to set controller, action and action parameters
        /// </summary>
        /// <param name="values">The route values associated with the current match</param>
        /// <param name="controller">Controller name</param>
        /// <param name="action">Action name</param>
        /// <param name="slug">URL slug</param>
        /// <param name="parameters">Action parameters</param>
        protected virtual void RouteToAction(RouteValueDictionary values, string controller, string action, string slug, params (string Key, object Value)[] parameters)
        {
            values[AnilRoutingDefaults.RouteValue.Controller] = controller;
            values[AnilRoutingDefaults.RouteValue.Action] = action;
            values[AnilRoutingDefaults.RouteValue.SeName] = slug;
            foreach (var (key, value) in parameters)
            {
                values[key] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a set of transformed route values that will be used to select an action
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="routeValues">The route values associated with the current match</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the set of values
        /// </returns>
        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary routeValues)
        {
            //get values to transform for action selection
            var values = new RouteValueDictionary(routeValues);
            if (values is null)
                return values;

            if (!values.TryGetValue(AnilRoutingDefaults.RouteValue.SeName, out var slug))
                return values;

            //find record by the URL slug
            if (await _urlRecordService.GetBySlugAsync(slug.ToString()) is not UrlRecord urlRecord)
                return values;

            //allow third-party handlers to select an action by the found URL record
            var routingEvent = new GenericRoutingEvent(httpContext, values, urlRecord);
            await _eventPublisher.PublishAsync(routingEvent);
            if (routingEvent.Handled)
                return values;

            //then try to select an action by the found URL record and the catalog path
            var catalogPath = values.TryGetValue(AnilRoutingDefaults.RouteValue.CatalogSeName, out var catalogPathValue)
                ? catalogPathValue.ToString()
                : string.Empty;

            //finally, select an action by the URL record only
            await SingleSlugRoutingAsync(httpContext, values, urlRecord, catalogPath);

            return values;
        }

        #endregion
    }
}