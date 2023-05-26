using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Anil.Core;
using Anil.Core.Security;
using Microsoft.Extensions.Configuration;

namespace Anil.Web.framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly CookieSettings _cookieSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public WebWorkContext(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _cookieSettings = configuration.Get<CookieSettings>()?? new CookieSettings();
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

    }
}