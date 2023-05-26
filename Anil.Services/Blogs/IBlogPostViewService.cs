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
    public partial interface IBlogPostViewService : IBaseService<BlogPostView>
    {
        BlogPostView FindByPostId(int postId);
    }
}