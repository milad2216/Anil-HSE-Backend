using Anil.Core.Common;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Customers;
using Anil.Core.Domain.Duties;
using Anil.Services.Duties;
using Anil.Services.Seo;
using Anil.Web.Areas.Admin.Models.Users;
using Anil.Web.Framework.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Website.Area.Api.ViewModel.Blogs;
using Website.Area.Api.ViewModel.Duties;

namespace Website.Area.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DutyController : ApiBaseController<Duty, DutyViewModel, DutyViewModel, IDutyService>
    {
        private readonly IUrlRecordService _urlRecordService;
        public DutyController(IConfiguration config, IDutyService service, IUrlRecordService urlRecordService) : base(config, service)
        {
            _urlRecordService = urlRecordService;
        }

        public override JsonResult Update(DutyViewModel model)
        {
            var entitiy = _service.FindById(model.Id);
            var result = _service.Edit(model.ToEntity<Duty, DutyViewModel>(entitiy));
            if (result.Status == 0 && !string.IsNullOrEmpty(model.Url))
            {
                var urlRecord = _urlRecordService.GetActiveSlugEntity(model.Id, "Duty");
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
                            EntityName = "Duty",
                            IsActive = true,
                            Slug = model.Url
                        });
                    }
                }
            }
            return new JsonResult(result);
        }

        public override JsonResult Create(DutyViewModel model)
        {
            var result = _service.Create(model.ToEntity<Duty>());
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
                        EntityId = int.Parse(result.ID),
                        EntityName = "Duty",
                        IsActive = true,
                        Slug = model.Url
                    });
                }
            }
            return new JsonResult(result);
        }

        public override JsonResult GetAll(int start = 0, int length = 10)
        {
            var entities = _service.GetAll().Skip(start).Take(length).ToList().ToModel<Duty, DutyViewModel>().Select(s =>
            {
                s.Url = _urlRecordService.GetActiveSlug(s.Id, "Duty");
                return s;
            }).ToList(); ;
            var total = _service.GetAll().Count();
            return new JsonResult(new CFResult<DutyViewModel>
            {
                Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Success,
                Data = entities,
                RecordsTotal = total,
                RecordsFiltered = total
            });
        }
    }
}
