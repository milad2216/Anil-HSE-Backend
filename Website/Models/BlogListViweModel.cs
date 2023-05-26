using System.Collections.Generic;
using Website.Area.Api.ViewModel.Blogs;

namespace Website.Models
{
    public class BlogListViweModel
    {
        public List<BlogPostViewModel> Blogs { get; set; }
        public BlogPostViewModel SelectedByEditor { get; set; }
        public List<BlogPostViewModel> MostViewedBlogs { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; } = 12;
    }
}
