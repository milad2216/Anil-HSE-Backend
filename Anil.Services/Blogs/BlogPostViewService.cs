using Anil.Core.Caching;
using Anil.Core;
using Anil.Core.Domain.Blogs;
using Anil.Data;
using Anil.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anil.Services.Blogs
{
    public partial class BlogPostViewService : BaseService<BlogPostView>, IBlogPostViewService
    {
        #region Fields

        private readonly IRepository<BlogPostView> _blogPostViewRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion
        public BlogPostViewService(IRepository<BlogPostView> blogPostViewRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext) : base(blogPostViewRepository, workContext)
        {
            _blogPostViewRepository = blogPostViewRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        /// <summary>
        /// Gets and cache three last blog for main page
        /// </summary>
        /// <returns>
        /// The result contains the blogs
        /// </returns>
        public virtual BlogPostView FindByPostId(int postId)
        {
            return _blogPostViewRepository.GetAll().FirstOrDefault(p => p.BlogPostId == postId) ?? new BlogPostView { BlogPostId = postId, Views = 0 };
        }
    }
}
