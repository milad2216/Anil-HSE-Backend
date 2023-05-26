using System.Threading.Tasks;
using Anil.Core.Caching;
using Anil.Core.Domain.Blogs;
using Anil.Core.Events;
using Anil.Services.Events;

namespace Website.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer
    {
        #region Fields

        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        #endregion

        #region Languages

        /// <returns>A task that represents the asynchronous operation</returns>
        //public async Task HandleEventAsync(EntityInsertedEvent<Language> eventMessage)
        //{
        //    //clear all localizable models
        //    await _staticCacheManager.RemoveByPrefixAsync(AnilModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        //    await _staticCacheManager.RemoveByPrefixAsync(AnilModelCacheDefaults.CategoryAllPrefixCacheKey);
        //    await _staticCacheManager.RemoveByPrefixAsync(AnilModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
        //}

        #endregion
    }
}