using Anil.Web.Framework.Models;
using Anil.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;

namespace Website.Area.Api.ViewModel.Duties
{
    public partial record DutyViewModel : BaseAnilEntityModel
    {
        #region Ctor

        public DutyViewModel()
        {
        }

        #endregion

        #region Properties

        public int? ParentId { get; set; }
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

        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the blog tags
        /// </summary>
        public string Tags { get; set; }

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

        public bool? ShowInTopSix { get; set; }

        public bool? ShowInFooter { get; set; }

        /// <summary>
        /// Gets or sets the url of entity
        /// </summary>
        public string Url { get; set; }

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