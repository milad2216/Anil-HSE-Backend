using Anil.Core.Caching;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Duties;
using Anil.Services.Blogs;
using Anil.Services.Duties;
using Anil.Services.Seo;
using Anil.Web.Framework.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Website.Area.Api.ViewModel.Blogs;
using Website.Area.Api.ViewModel.Duties;
using Website.Models;

namespace Website.Controllers
{
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IDutyService _dutyService;
        private readonly IBlogPostService _blogPostService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBlogPostViewService _blogPostViewService;

        public BlogController(IWebHostEnvironment appEnvironment, IDutyService dutyService,
            IBlogPostService blogPostService, IUrlRecordService urlRecordService,
            IBlogPostViewService blogPostViewService)
        {
            _appEnvironment = appEnvironment;
            _dutyService = dutyService;
            _blogPostService = blogPostService;
            _urlRecordService = urlRecordService;
            _blogPostViewService = blogPostViewService;
        }

        public IActionResult Index(int page = 1, string search = null)
        {
            var model = new BlogListViweModel();
            model.CurrentPage = page;
            var allBlogs = _blogPostService.GetAllList(getCacheKey: cache => default);
            model.TotalRecords = allBlogs.Count;
            model.Blogs = allBlogs.Skip((model.CurrentPage - 1) * model.ItemPerPage).Take(model.ItemPerPage).ToList().ToModel<BlogPost, BlogPostViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "BlogPost");
                if (string.IsNullOrEmpty(s.Url))
                {
                    s.Url = $"Blog/{s.Id}";
                }
                var view = _blogPostViewService.FindByPostId(s.Id);
                s.Views = view.Views;
                return s;
            }).ToList();
            var blogPostIds = _blogPostViewService.GetAll().OrderByDescending(o => o.Views).
                                        Take(2).Select(s => s.BlogPostId).ToList();
            model.MostViewedBlogs = allBlogs.Where(p => blogPostIds.Contains(p.Id)).ToList().ToModel<BlogPost, BlogPostViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "BlogPost");
                if (string.IsNullOrEmpty(s.Url))
                {
                    s.Url = $"Blog/{s.Id}";
                }
                var view = _blogPostViewService.FindByPostId(s.Id);
                s.Views = view.Views;
                return s;
            }).ToList();
            model.SelectedByEditor = allBlogs.Where(p => p.PickedByEditor == true).OrderByDescending(o => o.CreatedOnUtc).FirstOrDefault()?.ToModel<BlogPostViewModel>();
            if (model.SelectedByEditor != null)
            {
                var view = _blogPostViewService.FindByPostId(model.SelectedByEditor.Id);
                model.SelectedByEditor.Views = view.Views;

                model.SelectedByEditor.Url = _urlRecordService.GetActiveSlug(model.SelectedByEditor.Id, "BlogPost");
                if (string.IsNullOrEmpty(model.SelectedByEditor.Url))
                {
                    model.SelectedByEditor.Url = $"Blog/{model.SelectedByEditor.Id}";
                }
            }
            model.TotalPages = (int)(model.TotalRecords / model.ItemPerPage) + (model.TotalRecords % model.ItemPerPage != 0 ? 1 : 0);
            return View(model);
        }

        public IActionResult Details(int blogpostId)
        {
            var model = new BlogViweModel();
            model.Blog = _blogPostService.FindById(blogpostId).ToModel<BlogPostViewModel>();
            var blogView = _blogPostViewService.FindByPostId(blogpostId);
            blogView.Views++;
            if (blogView.Id > 0)
                _blogPostViewService.Edit(blogView);
            else
                _blogPostViewService.Create(blogView);
            model.Blog.Url = _urlRecordService.GetActiveSlug(model.Blog.Id, "BlogPost");
            if (string.IsNullOrEmpty(model.Blog.Url))
            {
                model.Blog.Url = $"Blog/{model.Blog.Id}";
            }
            model.Blog.Views = blogView.Views;
            model.LastSixDuties = _dutyService.GetMainPageDuties().ToModel<Duty, DutyViewModel>();
            model.LastTreeBlogs = _blogPostService.GetMainPageBlogs().ToModel<BlogPost, BlogPostViewModel>();
            model.CanonicalUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{model.Blog.Url}";
            return View(model);
        }
    }
}
