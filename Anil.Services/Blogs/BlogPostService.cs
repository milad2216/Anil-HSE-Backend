using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Duties;
using Anil.Data;
using Anil.Services.Base;
using Anil.Services.Duties;

namespace Anil.Services.Blogs
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class BlogPostService : BaseService<BlogPost>, IBlogPostService
    {
        #region Fields

        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public BlogPostService(IRepository<BlogPost> blogPostRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext) : base(blogPostRepository, workContext)
        {
            _blogPostRepository = blogPostRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="blogPosts">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteBlogPostsAsync(IList<BlogPost> blogPosts)
        {
            await _blogPostRepository.DeleteAsync(blogPosts);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="blogPostIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<BlogPost>> GetBlogPostsByIdsAsync(int[] blogPostIds)
        {
            return await _blogPostRepository.GetByIdsAsync(blogPostIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="blogPost">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertBlogPostAsync(BlogPost blogPost)
        {
            await _blogPostRepository.InsertAsync(blogPost);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="blogPost">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            await _blogPostRepository.UpdateAsync(blogPost);
        }

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
        public virtual async Task<IPagedList<BlogPost>> GetAllBlogPostsAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var blogPosts = (await _blogPostRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.CreatedOnUtc);

                return query;
            }, cache => default)).AsQueryable();

            var result = blogPosts.ToList();

            return new PagedList<BlogPost>(result, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets and cache three last blog for main page
        /// </summary>
        /// <returns>
        /// The result contains the blogs
        /// </returns>
        public virtual List<BlogPost> GetMainPageBlogs()
        {
            return _staticCacheManager.Get<List<BlogPost>>(AnilBlogDefaults.LastThreeBlogsKey, () =>
            {
                return _blogPostRepository.GetAll().Where(p => p.ShowInTopThree == true).OrderByDescending(o => o.CreatedOnUtc).Take(3).ToList();
            });
        }

        #endregion
    }
}
