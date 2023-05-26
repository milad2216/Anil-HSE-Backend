using Anil.Core.Domain.Blogs;
using Anil.Services.Caching;
using System.Threading.Tasks;

namespace Anil.Services.Blogs.Caching
{
    /// <summary>
    /// Represents an URL record cache event consumer
    /// </summary>
    public partial class BlogCommentCacheEventConsumer : CacheEventConsumer<BlogComment>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(BlogComment entity)
        {
            await RemoveAsync(AnilBlogDefaults.BlogCommentCacheKey, entity.Id, entity.BlogPostId);
        }
    }
}
