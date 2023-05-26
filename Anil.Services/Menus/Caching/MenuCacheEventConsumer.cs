using Anil.Core.Domain.Menus;
using Anil.Services.Caching;
using System.Threading.Tasks;

namespace Anil.Services.Menus.Caching
{
    /// <summary>
    /// Represents an URL record cache event consumer
    /// </summary>
    public partial class MenuCacheEventConsumer : CacheEventConsumer<Menu>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Menu entity)
        {
            await RemoveAsync(AnilMenuDefaults.MenuCacheKey, entity.Id, entity.Title);
        }
    }
}
