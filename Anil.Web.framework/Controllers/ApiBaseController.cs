using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using Anil.Services.Seo;
using Anil.Core;
using Anil.Web.Framework.Models;
using Anil.Services.Base;
using Anil.Web.Framework.Infrastructure.Mapper.Extensions;
using Anil.Core.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website.Area.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiBaseController<TEntity, TRequestModel, TResponseModel, TService> : ControllerBase
        where TEntity : BaseEntity where TResponseModel : BaseAnilEntityModel
        where TRequestModel : BaseAnilEntityModel
        where TService : IBaseService<TEntity>
    {
        protected readonly IConfiguration _config;
        protected readonly TService _service;
        public ApiBaseController(IConfiguration config, TService service)
        {
            _config = config;
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public virtual JsonResult Create(TRequestModel model)
        {
            return new JsonResult(_service.Create(model.ToEntity<TEntity>()));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public virtual JsonResult Update(TRequestModel model)
        {
            var entitiy = _service.FindById(model.Id);
            return new JsonResult(_service.Edit(model.ToEntity(entitiy)));
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public virtual JsonResult Delete(int id)
        {
            var entity = _service.FindById(id);
            return new JsonResult(_service.Delete(entity));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual JsonResult GetAll(int start = 0, int length = 10)
        {
            var entities = _service.GetAll().Skip(start).Take(length).ToList();
            var total = _service.GetAll().Count();
            return new JsonResult(new CFResult<TResponseModel>
            {
                Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Success,
                Data = entities.ToModel<TEntity, TResponseModel>(),
                RecordsTotal = total,
                RecordsFiltered = total
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public virtual JsonResult GetById(int id)
        {
            var entitiy = _service.FindById(id);
            return new JsonResult(new CFResult<TResponseModel>
            {
                Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Success,
                Extra = entitiy.ToModel<TResponseModel>(),
            });
        }
    }
}
