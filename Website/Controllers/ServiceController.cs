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
    public class ServiceController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IDutyService _dutyService;
        private readonly IBlogPostService _blogPostService;
        private readonly IUrlRecordService _urlRecordService;

        public ServiceController(IWebHostEnvironment appEnvironment, IDutyService dutyService, IBlogPostService blogPostService, IUrlRecordService urlRecordService)
        {
            _appEnvironment = appEnvironment;
            _dutyService = dutyService;
            _blogPostService = blogPostService;
            _urlRecordService = urlRecordService;
        }

        public IActionResult Index(string search)
        {
            var model = new ServiceListViweModel();
            model.Services = _dutyService.GetAllList(getCacheKey: cache => default).ToList().ToModel<Duty, DutyViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "Duty");
                if(string.IsNullOrEmpty(s.Url))
                {
                    s.Url = $"Service/{s.Id}";
                }
                return s;
            }).ToList();
            return View(model);
        }

        public IActionResult Details(int dutyId)
        {
            var model = new ServiceViweModel();
            model.Service = _dutyService.FindById(dutyId).ToModel<DutyViewModel>();
            model.Service.Url = _urlRecordService.GetActiveSlug(model.Service.Id, "Duty");
            if (string.IsNullOrEmpty(model.Service.Url))
            {
                model.Service.Url = $"Service/{model.Service.Id}";
            }
            model.LastSixDuties = _dutyService.GetMainPageDuties().ToModel<Duty, DutyViewModel>();
            model.LastTreeBlogs = _blogPostService.GetMainPageBlogs().ToModel<BlogPost, BlogPostViewModel>();
            model.CanonicalUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{model.Service.Url}";
            return View(model);
        }
    }
}
