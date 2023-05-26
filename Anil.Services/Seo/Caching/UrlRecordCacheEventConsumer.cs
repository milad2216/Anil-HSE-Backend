using Anil.Core.Domain.Seo;
using Anil.Services.Caching;
using System.Threading.Tasks;

namespace Anil.Services.Seo.Caching
{
    /// <summary>
    /// Represents an URL record cache event consumer
    /// </summary>
    public partial class UrlRecordCacheEventConsumer : CacheEventConsumer<UrlRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UrlRecord entity)
        {
            await RemoveAsync(AnilSeoDefaults.UrlRecordCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(AnilSeoDefaults.UrlRecordBySlugCacheKey, entity.Slug);
        }
    }
}
