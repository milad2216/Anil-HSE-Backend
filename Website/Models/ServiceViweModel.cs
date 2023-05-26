using System;
using System.Collections.Generic;
using Website.Area.Api.ViewModel.Blogs;
using Website.Area.Api.ViewModel.Duties;

namespace Website.Models
{
    public class ServiceViweModel
    {
        public DutyViewModel Service { get; set; }
        public List<DutyViewModel> LastSixDuties { get; set; }
        public List<BlogPostViewModel> LastTreeBlogs { get; set; }
        public string CanonicalUrl { get; set; }
    }
}
