using System.Collections.Generic;
using Website.Area.Api.ViewModel.Blogs;
using Website.Area.Api.ViewModel.Duties;

namespace Website.Models
{
    public class HomeViewModel
    {
        public List<DutyViewModel> LastSixDuties { get; set; }
        public List<BlogPostViewModel> LastTreeBlogs { get; set; }
    }
}
