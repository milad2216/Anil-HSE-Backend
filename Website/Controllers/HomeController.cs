using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Duties;
using Anil.Services.Blogs;
using Anil.Services.Duties;
using Anil.Services.Seo;
using Anil.Web.Framework.Infrastructure.Mapper.Extensions;
using CKSource.CKFinder.Connector.Config.Nodes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using Website.Area.Api.ViewModel.Blogs;
using Website.Area.Api.ViewModel.Duties;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IDutyService _dutyService;
        private readonly IBlogPostService _blogPostService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBlogPostViewService _blogPostViewService;
        public HomeController(IWebHostEnvironment appEnvironment, IDutyService dutyService, 
                IBlogPostService blogPostService, IUrlRecordService urlRecordService, IBlogPostViewService blogPostViewService)
        {
            _appEnvironment = appEnvironment;
            _dutyService = dutyService;
            _blogPostService = blogPostService;
            _urlRecordService = urlRecordService;
            _blogPostViewService = blogPostViewService;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            model.LastSixDuties = _dutyService.GetMainPageDuties().ToModel<Duty, DutyViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "Duty");
                return s;
            }).ToList();
            model.LastTreeBlogs = _blogPostService.GetMainPageBlogs().ToModel<BlogPost, BlogPostViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "BlogPost");
                var view = _blogPostViewService.FindByPostId(s.Id);
                s.Views = view.Views;
                return s;
            }).ToList(); ;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UploadFile(IFormFile upload, string prefix)
        {
            var uploadPath = Path.Combine(_appEnvironment.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if(string.IsNullOrEmpty(prefix))
            {
                prefix = "general";
            }

            string filePath = "";
            if (upload.Length > 0)
            {
                filePath = Path.Combine(uploadPath, prefix + "-" + DateTime.Now.Ticks + "-" + upload.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    upload.CopyTo(fileStream);
                }
            }
            var baseUri = $"{Request.Scheme}://{Request.Host}";
            return new JsonResult(new
            {
                url = $"{baseUri}/Uploads/{Path.GetFileName(filePath)}",
                fileName = Path.GetFileName(filePath),
                uploaded = 1
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}