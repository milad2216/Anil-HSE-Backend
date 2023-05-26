using Anil.Web.Framework.Models;
using Anil.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace Website.Area.Api.ViewModel.Blogs
{
    public partial record BlogPostViewModel : BaseAnilEntityModel
    {
        #region Ctor

        public BlogPostViewModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value indicating whether this blog post should be included in sitemap
        /// </summary>
        public bool IncludeInSitemap { get; set; }

        /// <summary>
        /// Gets or sets the blog post title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the blog post body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the blog post overview. If specified, then it's used on the blog page instead of the "Body"
        /// </summary>
        public string BodyOverview { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the blog post comments are allowed 
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// Gets or sets the blog tags
        /// </summary>
        public string Tags { get; set; }

        public string Writer { get; set; }

        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the url of entity
        /// </summary>
        public string Url { get; set; }

        public long Views { get; set; }

        public int DaysAgo
        {
            get
            {
                return (int)(DateTime.Now - CreatedOnUtc).TotalDays;
            }
        }

        public string DaysAgoString
        {
            get
            {
                var years = (int)(DaysAgo / 365);
                if ( years > 0)
                {
                    return $"{years} سال پیش";
                }
                var month = (int)(DaysAgo / 30);
                if (month > 0)
                {
                    return $"{month} ماه پیش";
                }
                var week = (int)(DaysAgo / 7);
                if (week > 0)
                {
                    return $"{week} هفته پیش";
                }
                if (DaysAgo > 0)
                {
                    return $"{DaysAgo} روز پیش";
                }
                return "امروز";
            }
        }


        public string FormatedViews
        {
            get
            {
                return Views.ToString("N0", new NumberFormatInfo()
                {
                    NumberGroupSizes = new[] { 3 },
                    NumberGroupSeparator = ","
                });
            }
        }

        public string DisplayDateTime
        {
            get
            {
                PersianCalendar pc = new PersianCalendar();
                if (CreatedOnUtc < pc.MinSupportedDateTime)
                {
                    return "";
                }
                return pc.GetYear(CreatedOnUtc).ToString("0000") + "/" +
                        pc.GetMonth(CreatedOnUtc).ToString("00") + "/" +
                        pc.GetDayOfMonth(CreatedOnUtc).ToString("00");
            }
        }
        #endregion
    }
}