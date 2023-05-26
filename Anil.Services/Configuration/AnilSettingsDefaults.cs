using Anil.Core.Caching;
using Anil.Core.Domain.Configuration;

namespace Anil.Services.Configuration
{
    /// <summary>
    /// Represents default values related to settings
    /// </summary>
    public static partial class AnilSettingsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new("Anil.setting.all.dictionary.", AnilEntityCacheDefaults<Setting>.Prefix);

        #endregion
    }
}