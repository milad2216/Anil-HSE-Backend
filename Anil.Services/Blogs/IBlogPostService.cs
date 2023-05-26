using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Duties;
using Anil.Services.Base;

namespace Anil.Services.Blogs
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface IBlogPostService : IBaseService<BlogPost>
    {
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="blogPosts">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteBlogPostsAsync(IList<BlogPost> blogPosts);

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="blogPostIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        Task<IList<BlogPost>> GetBlogPostsByIdsAsync(int[] blogPostIds);

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="blogPost">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertBlogPostAsync(BlogPost blogPost);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="blogPost">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateBlogPostAsync(BlogPost blogPost);

        /// <summary>
        /// Gets all URL records
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="languageId">Language ID; "null" to load records with any language; "0" to load records with standard language only; otherwise to load records with specify language ID only</param>
        /// <param name="isActive">A value indicating whether to get active records; "null" to load all records; "false" to load only inactive records; "true" to load only active records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL records
        /// </returns>
        Task<IPagedList<BlogPost>> GetAllBlogPostsAsync(string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets and cache three last blog for main page
        /// </summary>
        /// <returns>
        /// The result contains the blogs
        /// </returns>
        List<BlogPost> GetMainPageBlogs();
    }
}