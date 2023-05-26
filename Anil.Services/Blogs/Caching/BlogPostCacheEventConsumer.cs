using Anil.Core.Domain.Blogs;
using Anil.Services.Caching;
using Anil.Services.Duties;
using System.Threading.Tasks;

namespace Anil.Services.Blogs.Caching
{
    /// <summary>
    /// Represents an URL record cache event consumer
    /// </summary>
    public partial class BlogPostCacheEventConsumer : CacheEventConsumer<BlogPost>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(BlogPost entity)
        {
            await base.ClearCacheAsync(entity);
            await RemoveAsync(AnilBlogDefaults.LastThreeBlogsKey);
            await RemoveAsync(AnilBlogDefaults.BlogPostCacheKey, entity.Id, entity.Title);
        }
    }
}
