using Anil.Core.Common;
using Anil.Core.Domain.Blogs;
using Anil.Services.Blogs;
using Anil.Services.Seo;
using Anil.Web.Framework.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Website.Area.Api.ViewModel.Blogs;

namespace Website.Area.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ApiBaseController<BlogPost, BlogPostViewModel, BlogPostViewModel, IBlogPostService>
    {
        private readonly IUrlRecordService _urlRecordService;
        public BlogController(IConfiguration config, IBlogPostService service, IUrlRecordService urlRecordService) : base(config, service)
        {
            _urlRecordService = urlRecordService;
        }

        public override JsonResult Update(BlogPostViewModel model)
        {
            var entitiy = _service.FindById(model.Id);
            var result = _service.Edit(model.ToEntity<BlogPost, BlogPostViewModel>(entitiy));
            if (result.Status == 0 && !string.IsNullOrEmpty(model.Url))
            {
                var urlRecord = _urlRecordService.GetActiveSlugEntity(model.Id, "BlogPost");
                if (urlRecord != null)
                {
                    if (urlRecord.Slug != model.Url)
                    {
                        var find = _urlRecordService.GetBySlug(model.Url);
                        if (find != null)
                        {
                            result.Message = "آدرس وارد شده برای Url تکراری است";
                        }
                        else
                        {
                            urlRecord.Slug = model.Url;
                            _urlRecordService.Edit(urlRecord);
                        }
                        // TODO save old url for redirection
                    }
                }
                else
                {
                    var find = _urlRecordService.GetBySlug(model.Url);
                    if (find != null)
                    {
                        result.Message = "آدرس وارد شده برای Url تکراری است";
                    }
                    else
                    {
                        _urlRecordService.Create(new Anil.Core.Domain.Seo.UrlRecord
                        {
                            EntityId = model.Id,
                            EntityName = "BlogPost",
                            IsActive = true,
                            Slug = model.Url
                        });
                    }
                }
            }
            return new JsonResult(result);
        }

        public override JsonResult Create(BlogPostViewModel model)
        {
            var result = _service.Create(model.ToEntity<BlogPost>());
            if (result.Status == 0 && !string.IsNullOrEmpty(model.Url))
            {
                var urlRecord = _urlRecordService.GetBySlug(model.Url);
                if (urlRecord != null)
                {
                    result.Message = "آدرس وارد شده برای Url تکراری است";
                }
                else
                {
                    _urlRecordService.Create(new Anil.Core.Domain.Seo.UrlRecord
                    {
                        EntityId= int.Parse(result.ID),
                        EntityName ="BlogPost",
                        IsActive=true,
                        Slug = model.Url
                    });
                }
            }
            return new JsonResult(result);
        }

        public override JsonResult GetAll(int start = 0, int length = 10)
        {
            var entities = _service.GetAll().Skip(start).Take(length).ToList().ToModel<BlogPost, BlogPostViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "BlogPost");
                return s;
            }).ToList(); ;
            var total = _service.GetAll().Count();
            return new JsonResult(new CFResult<BlogPostViewModel>
            {
                Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Success,
                Data = entities,
                RecordsTotal = total,
                RecordsFiltered = total
            });
        }
    }
}
