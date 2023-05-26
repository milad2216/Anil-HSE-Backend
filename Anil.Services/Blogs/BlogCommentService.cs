using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Blogs;
using Anil.Data;

namespace Anil.Services.Blogs
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class BlogCommentService : IBlogCommentService
    {
        #region Fields

        private readonly IRepository<BlogComment> _blogCommentRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public BlogCommentService(IRepository<BlogComment> blogCommentRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            _blogCommentRepository = blogCommentRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="blogComments">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteBlogCommentsAsync(IList<BlogComment> blogComments)
        {
            await _blogCommentRepository.DeleteAsync(blogComments);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="blogCommentIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<BlogComment>> GetBlogCommentsByIdsAsync(int[] blogCommentIds)
        {
            return await _blogCommentRepository.GetByIdsAsync(blogCommentIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="blogComment">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertBlogCommentAsync(BlogComment blogComment)
        {
            await _blogCommentRepository.InsertAsync(blogComment);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="blogComment">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateBlogCommentAsync(BlogComment blogComment)
        {
            await _blogCommentRepository.UpdateAsync(blogComment);
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
        public virtual async Task<IPagedList<BlogComment>> GetAllBlogCommentsAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var blogComments = (await _blogCommentRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.CreatedOnUtc);

                return query;
            }, cache => default)).AsQueryable();

            var result = blogComments.ToList();

            return new PagedList<BlogComment>(result, pageIndex, pageSize);
        }

        #endregion
    }
}
