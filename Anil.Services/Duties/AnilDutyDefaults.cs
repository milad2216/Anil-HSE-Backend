using System.Collections.Generic;
using Anil.Core.Caching;

namespace Anil.Services.Duties
{
    /// <summary>
    /// Represents default values related to SEO services
    /// </summary>
    public static partial class AnilDutyDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey LastSixDutyKey => new("Anil.duty.last.six");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey AllDutyKey => new("Anil.duty.all");

        #endregion
    }
}